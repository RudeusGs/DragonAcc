<template>
  <LoadingSpinner :isLoading="loading" />
  <div v-if="!loading" class="container">
    <div class="sidebar">
      <h3>Chọn game</h3>
      <ul class="game-list">
        <li
          v-for="game in gameOptions"
          :key="game"
          :class="{ active: selectedGameFilter === game }"
          @click="filterPostsByGame(game)"
        >
          {{ game }}
        </li>
      </ul>
      <h3 class="mt-3">Trạng thái</h3>
  <ul class="status-list">
    <li
      :class="{ active: selectedStatusFilter === 'All' }"
      @click="filterPostsByStatus('All')"
    >
      Tất cả
    </li>
    <li
      :class="{ active: selectedStatusFilter === 'Đang bán' }"
      @click="filterPostsByStatus('Đang bán')"
    >
      Đang bán
    </li>
    <li
      :class="{ active: selectedStatusFilter === 'Đã bán' }"
      @click="filterPostsByStatus('Đã bán')"
    >
      Đã bán
    </li>
  </ul>
    </div>
    <!-- Content Container -->
    <div class="content-container">
      <div class="left-column">
        
        <!-- Post Section -->
        <div class="post-container">
          <div class="post-header">
            <strong>Đăng bài</strong>
            <div class="menu-container">
              <i class="fas fa-ellipsis-h menu-icon" @click.stop="toggleMenu"></i>
              <div class="dropdown-menu" v-if="menuVisible">
                <ul>
                  <li class="drop-item" @click="selectOption('Liên minh huyền thoại')">Liên minh huyền thoại</li>
                  <li @click="selectOption('Pubg')">Pubg</li>
                  <li @click="selectOption('Ngọc rồng')">Ngọc rồng</li>
                  <li @click="selectOption('Valorant')">Valorant</li>
                  <li @click="selectOption('Tốc chiến')">Tốc chiến</li>
                </ul>
              </div>
            </div>
          </div>
          <div class="post-content">
            <img src="https://tintuc.dienthoaigiakho.vn/wp-content/uploads/2024/01/avatar-nam-nu-trang-2.jpg" alt="User Avatar" class="avatar" />
            <input
              type="text"
              placeholder="Mô tả về tài khoản của bạn"
              class="post-input"
              v-model="postText"
            />
          </div>
          <div class="input-container d-flex justify-content-center">
            <input type="text" class="linkedin-input" placeholder="Giá VNĐ" v-model="formData.price" />
          </div>          
          <div class="submitted-info" v-if="submittedData.length > 0">
            <div v-for="(data, index) in submittedData" :key="index" class="info-item">
              <h3 class="d-flex justify-content-center">Thông tin tài khoản {{data.selectedGame}}</h3>
              <p><strong>Tên Tài Khoản:</strong> {{ data.accountName }}</p>
              <p><strong>Mật khẩu:</strong> {{ data.password }}</p>
              <p><strong>Xếp hạng:</strong> {{ data.rank || 'Chưa xếp hạng' }}</p>
              <p v-if="data.championCount !== null"><strong>Số Lượng Tướng:</strong> {{ data.championCount }}</p>
              <p v-if="data.skinCount !== null"><strong>Số Lượng Trang Phục:</strong> {{ data.skinCount }}</p>
              <p v-if="data.gunSkinCount !== null"><strong>Số Lượng Skin Súng:</strong> {{ data.gunSkinCount }}</p>
              <p v-if="data.humanSkinCount !== null"><strong>Số Lượng Skin Nhân Vật:</strong> {{ data.humanSkinCount }}</p>
              <p v-if="data.server !== null"><strong>Máy Chủ:</strong> {{ data.server }}</p>
              <p v-if="data.planet"><strong>Hành Tinh:</strong> {{ data.planet }}</p>
            </div>
          </div>
          <div class="post-actions">
            <div class="icon-group d-flex justify-content-center">
              <label for="image-upload">
                <i class="fas fa-image"></i>
              </label>
              <input
                type="file"
                id="image-upload"
                class="image-input"
                @change="handleImageUpload"
                multiple
              />
            </div>
          </div>
          <div class="image-preview">
            <div class="image-wrapper" v-for="(image, index) in images" :key="index">
              <img :src="image" class="thumbnail" alt="Preview" />
              <span class="delete-icon" @click="removeImage(index)"><i class="fas fa-times"></i></span>
            </div>
          </div>
          <button class="post-button" @click="submitPost">Đăng</button>
        </div>
        <div class="modal-overlay" v-if="modalVisible">
          <div class="modal-content">
            <h2>Vui lòng nhập đúng thông tin</h2>
            <form @submit.prevent="submitForm">
              <div>
                <label for="accountName">Tên Tài Khoản:</label>
                <input type="text" id="accountName" v-model="formData.accountName" />
              </div>
              <div>
                <label for="password">Mật Khẩu:</label>
                <input type="password" id="password" v-model="formData.password" />
              </div>
              <div v-if="selectedGame === 'Liên minh huyền thoại' || selectedGame === 'Tốc chiến'">
                <div>
                  <label for="championCount">Số Lượng Tướng:</label>
                  <input type="number" id="championCount" v-model="formData.championCount" />
                </div>
                <div>
                  <label for="skinCount">Số Lượng Trang Phục:</label>
                  <input type="number" id="skinCount" v-model="formData.skinCount" />
                </div>
                <div>
                  <label for="rank">Hạng:</label>
                  <input type="text" id="rank" v-model="formData.rank" />
                </div>
              </div>
              <div v-else-if="selectedGame === 'Pubg' || selectedGame === 'Valorant'">
                <div>
                  <label for="gunSkinCount">Số Lượng Skin Súng:</label>
                  <input type="number" id="gunSkinCount" v-model="formData.gunSkinCount" />
                </div>
                <div v-if="selectedGame === 'Pubg'">
                  <label for="humanSkinCount">Số Lượng Skin Nhân Vật:</label>
                  <input type="number" id="humanSkinCount" v-model="formData.humanSkinCount" />
                </div>
                <div v-if="selectedGame === 'Valorant'">
                  <label for="championCount">Số Lượng Nhân Vật:</label>
                  <input type="number" id="championCount" v-model="formData.championCount" />
                </div>
                <div>
                  <label for="rank">Xếp hạng:</label>
                  <input type="text" id="rank" v-model="formData.rank" />
                </div>
              </div>
              <div v-else-if="selectedGame === 'Ngọc rồng'">
                <div>
                  <label for="server">Máy Chủ:</label>
                  <input type="number" id="server" v-model="formData.server" />
                </div>
                <div>
                  <label for="planet">Hành Tinh:</label>
                  <input type="text" id="planet" v-model="formData.planet" />
                </div>
              </div>
              <div class="modal-buttons">
                <button type="submit">Xác Nhận</button>
                <button type="button" @click="closeModal">Hủy</button>
              </div>
            </form>
          </div>
        </div>
    
        <!-- Post Album Section -->
        <div
        class="post-album-container"
        v-for="(post, index) in filteredPosts"
        :key="index"
      >
      <div :class="['status-badge', post.status === 'Đang bán' ? 'status-selling' : 'status-sold']">
        {{ post.status }}
      </div>
        <div class="user-info">
        <img src="https://tintuc.dienthoaigiakho.vn/wp-content/uploads/2024/01/avatar-nam-nu-trang-2.jpg" alt="User Avatar" class="avatar" />
        <div class="user-details">
          <span class="user-name">{{ post.fullName }}</span>
          <span class="post-type"> • {{ post.gameName }}</span>
          <div class="post-date">{{ formatDate(post.createdAt) }}</div>
        </div>
        </div>
  
        <div class="post-content">
          <p>{{ post.description }}</p>
        </div>
  
        <div class="post-images" v-if="post.images.length > 0">
         <div
        class="image-grid"
          :class="{
        'single-image': post.images.length === 1,
        'two-images': post.images.length === 2,
        'three-images': post.images.length === 3,
        'multiple-images': post.images.length > 3
      }"
    >
      <img
        v-for="(image, imgIndex) in post.images"
        :key="imgIndex"
        :src="getFullImageUrl(image)"
        alt="Product image"
        class="image-item"
      />
    </div>
  </div>
  
  <!-- Game-specific Attributes -->
  <div class="game-attributes">
    <!-- For Liên minh huyền thoại and Tốc chiến -->
    <div v-if="post.gameName === 'Liên minh huyền thoại' || post.gameName === 'Tốc chiến'">
      <p>Số Lượng Tướng: {{ post.championCount }}</p>
      <p>Số Lượng Trang Phục: {{ post.skinCount }}</p>
      <p>Hạng: {{ post.rank }}</p>
      <p class="price-item">Giá: {{ formatBalance(post.price) }}</p>
    </div>
    
    <!-- For Pubg and Valorant -->
    <div v-else-if="post.gameName === 'Pubg' || post.gameName === 'Valorant'">
      <p>Số Lượng Skin Súng: {{ post.gunSkinCount }}</p>
      <p v-if="post.gameName === 'Pubg'">Số Lượng Skin Nhân Vật: {{ post.humanSkinCount }}</p>
      <p v-if="post.gameName === 'Valorant'">Số Lượng Nhân Vật: {{ post.championCount }}</p>
      <p>Xếp hạng: {{ post.rank }}</p>
      <p class="price-item">Giá: {{ formatBalance(post.price) }}</p>
    </div>
    
    <!-- For Ngọc rồng online -->
      <div v-else-if="post.gameName === 'Ngọc rồng online'">
          <p>Máy Chủ: {{ post.server }}</p>
          <p>Hành Tinh: {{ post.planet }}</p>
          <p class="price-item">Giá: {{ formatBalance(post.price) }}</p>
        </div>
      </div>
  
      <div class="post-footer">
        <div class="reactions">
          <span class="reaction"><i class="fas fa-heart" style="margin-right: 5px;"></i>Thích</span>
          <span class="reaction"><i class="fas fa-comment" style="margin-right: 5px;" ></i>Bình luận</span>
          <span 
            class="reaction" 
            v-if="post.userId !== store.user?.id"
            @click="confirmAndPurchase(post)">
            <i class="fas fa-shopping-cart" style="margin-right: 5px;"></i>Mua
          </span>         
        </div>
      </div>
      </div>      
        <div class="modal-overlay success-modal" v-if="successModalVisible">
          <div class="modal-content">
            <i class="fas fa-check-circle success-icon"></i>
            <h2>Đăng bài thành công!</h2>
            <p>Bài đăng của bạn đã được gửi thành công.</p>
            <div class="modal-buttons">
              <button type="button" @click="closeSuccessModal" class="close-button">Đóng</button>
            </div>
          </div>
        </div>        
    </div>
    <div class="right-column">
      <div class="page-container">
        <div class="profile-header">
          <img src="https://tintuc.dienthoaigiakho.vn/wp-content/uploads/2024/01/avatar-nam-nu-trang-2.jpg" alt="User Avatar" />
          <h2>{{fullname}}</h2>
          <p>Messages • Notifications</p>
        </div>
        <div class="menu">
          <div class="menu-item">Publish</div>
          <div class="menu-item">Photo</div>
          <div class="menu-item">Live</div>
          <div class="menu-item">Invite</div>
        </div>
        <div class="stats">
          <button class="likes">Likes</button>
          <button class="views">Views</button>
        </div>
        <div class="likes-count">884</div>
        <div class="new-likes">35 New Likes This Week</div>
      </div>
    </div>
  </div>
</div>
</template>
<script setup lang="ts">
import LoadingSpinner from '@/components/LoadingSpinner.vue';
import { ref, onMounted, onBeforeUnmount, computed } from 'vue';
import {
  addLol_Game,
  addNgocRong_Game,
  addPubg_Game,
  addTocChien_Game,
  addValorant_Game,
} from '@/api/addgameaccount.api';
import PurchasedgameaccountAPI from '@/api/purchased.api';
import { userStore } from '@/stores/auth';
import getallgameaccountAPI from '@/api/getallgameaccount.api';
import getfullname from '@/api/getfullname.api';

const postText = ref('');
const store = userStore();
const images = ref<string[]>([]);
const loading = ref(true);
const imageFiles = ref<File[]>([]);
const menuVisible = ref(false);
const loadingImages = ref<boolean[]>([]);
const modalVisible = ref(false);
const successModalVisible = ref(false);
const selectedGameFilter = ref('All');
const gameOptions = ref(['All', 'Liên minh huyền thoại', 'Pubg', 'Ngọc rồng online', 'Valorant', 'Tốc chiến']);
const selectedGame = ref('');
const formData = ref<any>({});
const submittedData = ref<any[]>([]);
const posts = ref<any[]>([]);
  const selectedStatusFilter = ref('All');
const filterPostsByStatus = (status: string) => {
  selectedStatusFilter.value = status;
};

const toggleMenu = () => {
  menuVisible.value = !menuVisible.value;
};

const selectOption = (option: string) => {
  selectedGame.value = option;
  modalVisible.value = true;
  menuVisible.value = false;
  formData.value = {
    gameName: '',
    accountName: '',
    password: '',
    price: '',
    championCount: null,
    skinCount: null,
    rank: '',
    gunSkinCount: null,
    humanSkinCount: null,
    server: null,
    planet: '',
  };
};
const formatBalance = (balanceString: string) => {
  const balance = parseFloat(balanceString);
  if (isNaN(balance)) return '0 VNĐ';
  return balance.toLocaleString('vi-VN') + ' VNĐ';
};
const closeModal = () => {
  modalVisible.value = false;
};
const fullname = computed(() => store.user?.fullName || 'User');
const submitForm = () => {
  submittedData.value.push({
    ...formData.value,
    selectedGame: selectedGame.value,
  });
  modalVisible.value = false;
};
const closeSuccessModal = () => {
  successModalVisible.value = false;
};
const formatDate = (dateString: string) => {
  if (!dateString) return '';
  const date = new Date(dateString);
  const now = new Date();
  const diffInSeconds = Math.floor((now.getTime() - date.getTime()) / 1000);

  if (diffInSeconds < 60) return 'Vừa xong';
  if (diffInSeconds < 3600) return `${Math.floor(diffInSeconds / 60)} phút trước`;
  if (diffInSeconds < 86400) return `${Math.floor(diffInSeconds / 3600)} giờ trước`;
  if (diffInSeconds < 2592000) return `${Math.floor(diffInSeconds / 86400)} ngày trước`;
  
  return date.toLocaleDateString('vi-VN');
};
const confirmAndPurchase = async (post: any) => {
  const userConfirmed = window.confirm(`Bạn có chắc chắn muốn mua tài khoản ${post.gameName} của ${post.fullName}?`);
  if (userConfirmed) {
    await handlePurchase(post);
  }
};
const handlePurchase = async (post: any) => {
  try {
    const userId = store.user?.id;
    if (!userId) {
      alert("Please log in to proceed with the purchase.");
      return;
    }
    const gameAccountId = post.id;
    if (!gameAccountId) {
      alert("Game account ID is missing in post data. Please check the source.");
      return;
    }

    const gameName = post.gameName;
    let response;
    switch (gameName) {
      case 'Liên minh huyền thoại':
        response = await PurchasedgameaccountAPI.purchasedLOL(gameAccountId, userId);
        break;
      case 'Pubg':
        response = await PurchasedgameaccountAPI.purchasedPubg(gameAccountId, userId);
        break;
      case 'Ngọc rồng online':
        response = await PurchasedgameaccountAPI.purchasedNgocRong(gameAccountId, userId);
        break;
      case 'Tốc chiến':
        response = await PurchasedgameaccountAPI.purchasedTocChien(gameAccountId, userId);
        break;
      case 'Valorant':
        response = await PurchasedgameaccountAPI.purchasedValorant(gameAccountId, userId);
        break;
      default:
        alert("Invalid game selection for purchase.");
        return;
    }
    if (response && response.data.result.isSuccess) {
      alert("Purchase successful!");
      fetchAllPosts();
    } else {
      alert(response.data.result.message || "Purchase failed. Please try again.");
    }
  } catch (error) {
    console.error("Error during purchase:", error);
    alert("An error occurred while processing the purchase.");
  }
};

const resetForm = () => {
  formData.value = {};
  images.value = [];
  imageFiles.value = [];
  selectedGame.value = '';
  postText.value = '';
  submittedData.value = [];
};
const fetchData = async () => {
  loading.value = true;
  await new Promise((resolve) => setTimeout(resolve, 2000));
  loading.value = false;
};

const submitPost = async () => {
  try {
    if (!selectedGame.value) {
      alert('Vui lòng chọn một trò chơi.');
      return;
    }

    const formDataToSend = new FormData();

    formDataToSend.append('GameName', selectedGame.value);
    formDataToSend.append('AccountName', formData.value.accountName || '');
    formDataToSend.append('Password', formData.value.password || '');
    formDataToSend.append('Description', postText.value || '');
    formDataToSend.append('Price', formData.value.price || '0');

    imageFiles.value.forEach((file) => {
      formDataToSend.append('Files', file);
    });

    switch (selectedGame.value) {
      case 'Liên minh huyền thoại':
        formDataToSend.append('ChampionCount', formData.value.championCount || '0');
        formDataToSend.append('SkinCount', formData.value.skinCount || '0');
        formDataToSend.append('Rank', formData.value.rank || '');
        await addLol_Game(formDataToSend);
        break;

      case 'Tốc chiến':
        formDataToSend.append('ChampionCount', formData.value.championCount || '0');
        formDataToSend.append('SkinCount', formData.value.skinCount || '0');
        formDataToSend.append('Rank', formData.value.rank || '');
        await addTocChien_Game(formDataToSend);
        break;

      case 'Pubg':
        formDataToSend.append('GunSkinCount', formData.value.gunSkinCount || '0');
        formDataToSend.append('HumanSkinCount', formData.value.humanSkinCount || '0');
        formDataToSend.append('Rank', formData.value.rank || '');
        await addPubg_Game(formDataToSend);
        break;

      case 'Valorant':
        formDataToSend.append('GunSkinCount', formData.value.gunSkinCount || '0');
        formDataToSend.append('ChampionCount', formData.value.championCount || '0'); 
        formDataToSend.append('Rank', formData.value.rank || '');
        await addValorant_Game(formDataToSend);
        break;

      case 'Ngọc rồng':
        formDataToSend.append('Server', formData.value.server || '');
        formDataToSend.append('Planet', formData.value.planet || '');
        await addNgocRong_Game(formDataToSend);
        break;

      default:
        alert('Trò chơi không hợp lệ.');
        return;
    }
    successModalVisible.value = true;
    resetForm();
    await fetchAllPosts();
  } catch (error) {
    console.error(error);
    alert('Có lỗi xảy ra khi đăng bài.');
  }
};
const getFullImageUrl = (imageString: string | null) => {
  if (!imageString) {
    return 'https://via.placeholder.com/550x500';
  }
  const baseUrl = 'https://localhost:7071/';
  const firstImage = imageString.split(';')[0];
  return `${baseUrl}${firstImage}`;
};
const imageLoaded = (index: number) => {
      loadingImages.value[index] = false;
    };
const removeImage = (index: number) => {
  images.value.splice(index, 1);
  imageFiles.value.splice(index, 1);
};

const handleImageUpload = (event: Event) => {
  const files = (event.target as HTMLInputElement).files;
  if (files) {
    Array.from(files).forEach((file: File) => {
      imageFiles.value.push(file);
      const reader = new FileReader();
      reader.onload = (e) => {
        if (e.target && e.target.result) {
          images.value.push(e.target.result as string);
        }
      };
      reader.readAsDataURL(file);
    });
  }
};
const filteredPosts = computed(() => {
  let postsFilteredByGame = posts.value.filter(item => item.status !== 'Đang chờ duyệt');

  if (selectedGameFilter.value !== 'All') {
    postsFilteredByGame = postsFilteredByGame.filter(post => post.gameName === selectedGameFilter.value);
  }

  if (selectedStatusFilter.value !== 'All') {
    postsFilteredByGame = postsFilteredByGame.filter(post => post.status === selectedStatusFilter.value);
  }

  return postsFilteredByGame;
});


// Set selected game filter
const filterPostsByGame = (game: string) => {
  selectedGameFilter.value = game;
};
const fetchAllPosts = async () => {
  try {
    const [
      lolResponse,
      ngocRongResponse,
      pubgResponse,
      tocChienResponse,
      valorantResponse,
    ] = await Promise.all([
      getallgameaccountAPI.getAllLOL(),
      getallgameaccountAPI.getAllNgocRong(),
      getallgameaccountAPI.getAllPubg(),
      getallgameaccountAPI.getAllTocChien(),
      getallgameaccountAPI.getAllValorant(),
    ]);

    const rawPosts = [
      ...(lolResponse.data.result.data || []),
      ...(ngocRongResponse.data.result.data || []),
      ...(pubgResponse.data.result.data || []),
      ...(tocChienResponse.data.result.data || []),
      ...(valorantResponse.data.result.data || []),
    ];

    // Fetch full names based on post `id` and add it to the post
    posts.value = await Promise.all(
      rawPosts.map(async (post) => {
        let fullName = '';
        try {
          // Call the appropriate `getfullname` function with `post.id` (the post ID)
          switch (post.gameName) {
            case 'Liên minh huyền thoại':
              fullName = (await getfullname.getfullnameLol(post.id)).data.result.data;
              break;
            case 'Ngọc rồng online':
              fullName = (await getfullname.getfullnameNgocRong(post.id)).data.result.data;
              break;
            case 'Pubg':
              fullName = (await getfullname.getfullnamePubg(post.id)).data.result.data;
              break;
            case 'Tốc chiến':
              fullName = (await getfullname.getfullnameTocChien(post.id)).data.result.data;
              break;
            case 'Valorant':
              fullName = (await getfullname.getfullnameValorant(post.id)).data.result.data;
              break;
            default:
              console.warn('Unknown game type:', post.gameName);
          }
        } catch (error) {
          console.error(`Error fetching full name for post ${post.id}:`, error);
        }
        return {
          ...post,
          fullName,  // Attach the retrieved fullName to each post
          images: post.image ? post.image.split(';') : [],
          createdAt: post.createdDate,
          updatedAt: post.updatedDate,
        };
      })
    );
    const selectedStatusFilter = ref('All');

const filterPostsByStatus = (status: string) => {
  selectedStatusFilter.value = status;
};

const filteredPosts = computed(() => {
  let postsFilteredByGame = posts.value;

  if (selectedGameFilter.value !== 'All') {
    postsFilteredByGame = postsFilteredByGame.filter(post => post.gameName === selectedGameFilter.value);
  }

  if (selectedStatusFilter.value !== 'All') {
    postsFilteredByGame = postsFilteredByGame.filter(post => post.status === selectedStatusFilter.value);
  }

  return postsFilteredByGame;
});

    posts.value.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
  } catch (error) {
    console.error('Error fetching posts:', error);
  }
};

onMounted(fetchAllPosts);

const handleClickOutside = (event: Event) => {
  const menu = document.querySelector('.menu-container');
  if (menu && !menu.contains(event.target as Node)) {
    menuVisible.value = false;
  }
};

onMounted(async () => {
  document.addEventListener('click', handleClickOutside);
  await fetchAllPosts();
  store.init();
  fetchData();
});
onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside);
});
</script>

<style scoped>
.container {
  display: flex;
  justify-content: center;
  align-items: flex-start;
  padding: 20px;
  box-sizing: border-box;
}
.content-container {
  margin-left: 20px;
  display: flex;
  flex-direction: row;
  gap: 20px;
}

.post-container {
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  padding: 15px;
  width: 570px;
  max-width: 100%;
}

.post-album-container {
  background-color: #ffffff;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 16px;
  margin-bottom: 16px;
  max-width: 600px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}
.left-column {
  display: flex;
  flex-direction: column;
  gap: 20px;
  max-width: 570px;
}
.post-container,
.post-album-container {
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  padding: 15px;
  max-width: 100%;
}
.right-column {
  width: 300px;
  
}
.page-container {
  background-color: white;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  padding: 20px;
  text-align: center;
  position: sticky;
  top: 88px; 
}

.profile-header {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 20px;
}
.sidebar {
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  padding: 15px;
  width: 300px;
  position: sticky;
  top: 88px; 
}

.sidebar h3 {
  font-size: 18px;
  color: #333;
  margin-bottom: 10px;
}

.game-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.game-list li {
  font-size: 14px;
  color: #0073b1;
  cursor: pointer;
  padding: 10px 0;
  border-bottom: 1px solid #ddd;
  transition: color 0.3s;
}

.game-list li:hover,
.game-list li.active {
  color: #005582;
  font-weight: bold;
}

/* Post Album Section */
.post-album-section {
  flex-grow: 1;
}
.profile-header img {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  margin-bottom: 10px;
}

.profile-header h2 {
  font-size: 18px;
  color: #333;
  margin-bottom: 5px;
}

.profile-header p {
  font-size: 12px;
  color: #888;
}

.menu {
  display: flex;
  justify-content: space-around;
  margin: 15px 0;
}

.menu-item {
  font-size: 14px;
  color: #666;
  cursor: pointer;
  transition: color 0.3s;
}

.menu-item:hover {
  color: #ff5722;
}

.stats {
  display: flex;
  justify-content: center;
  margin-top: 20px;
}
.post-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 15px;
  border-bottom: 1px solid #ddd;
  padding-bottom: 10px;
  position: relative;
}
.stats button {
  border: none;
  padding: 10px 20px;
  border-radius: 20px;
  cursor: pointer;
  font-size: 14px;
  margin: 0 5px;
}

.stats .likes {
  background-color: #ff5722;
  color: white;
}

.stats .views {
  background-color: #eee;
  color: #666;
}

.likes-count {
  font-size: 24px;
  font-weight: bold;
  color: #444;
  margin: 15px 0 5px;
}

.new-likes {
  font-size: 12px;
  color: #888;
}
.submitted-info {
  margin-top: 10px;
  padding: 10px;
  background-color: #f2f2f2;
  border-radius: 5px;
  border: 1px solid #ddd;
}

.info-item {
  margin-bottom: 10px;
}

.info-item h3 {
  font-size: 16px;
  color: #0073b1;
  margin-bottom: 5px;
}

.info-item p {
  font-size: 14px;
  color: #555;
  margin: 2px 0;
}

.avatar {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  margin-right: 12px;
}

.menu-container {
  position: absolute; /* Thêm vị trí tuyệt đối */
  top: 0; /* Đặt vị trí ở trên cùng */
  right: 0; /* Đặt vị trí ở góc phải */
}

.menu-icon {
  font-size: 18px;
  color: #666;
  cursor: pointer;
  z-index: 1;
}

.dropdown-menu {
  display: block;
  position: absolute;
  right: 0;
  top: 30px; /* Adjusted to be below the menu icon */
  background-color: white;
  border: 1px solid #e1e4e8;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.15);
  width: 250px;
  z-index: 10000;
  overflow: hidden; /* To keep the border-radius effect */
}
.dropdown-menu ul {
  list-style: none;
  margin: 0;
  padding: 5px 0;
}

.dropdown-menu li {
  padding: 12px 20px;
  cursor: pointer;
  font-size: 14px;
  color: #333;
  transition: background-color 0.2s, color 0.2s;
  display: flex;
  align-items: center;
}

.dropdown-menu li:hover {
  background-color: #f5f5f5;
  color: #0073b1;
}

.drop-item:last-child {
  border-bottom: none;
}
.post-content {
  display: flex;
  margin-bottom: 15px;
}
.post-input {
  width: 100%;
  outline: none;
  color: #666;
  font-size: 16px;
  padding: 10px;
  border: none;
  border-radius: 5px;
  transition: all 0.3s ease;
}

.post-input::placeholder {
  color: #bbb;
}

.icon-group label {
  font-size: 18px;
  color: #999;
  cursor: pointer;
  transition: color 0.3s;
}

.icon-group label:hover {
  color: #267fde;
}

.image-input {
  display: none;
}

.image-preview {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
  margin-top: 10px;
  margin-bottom: 9px;
}

.thumbnail {
  width: 60px;
  height: 60px;
  object-fit: cover;
  border-radius: 5px;
  border: 1px solid #ddd;
}

.post-button {
  width: 100%;
  background-color: #4498f1;
  color: white;
  border: none;
  padding: 5px;
  height: 30px;
  border-radius: 50px;
  font-size: 12px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.post-button:hover {
  background-color: #0872e2;
}

.user-info {
  display: flex;
  align-items: center;
  margin-bottom: 8px;
}

.user-details {
  font-size: 14px;
  color: #333;
}

.user-name {
  font-weight: bold;
}

.post-type {
  color: #555;
}

.post-date {
  font-size: 12px;
  color: #999;
}
.price-item{
  color: #006bdd;
  font-weight: 500;
  display: flex;
  justify-content: center;
}
.post-content p {
  font-size: 14px;
  color: #333;
  margin-bottom: 12px;
}

.post-images {
  display: flex;
  flex-direction: wrap;
  gap: 5px;
  margin-top: 10px;
}

.image-row {
  display: flex;
  gap: 5px;
  margin-bottom: 5px;
}
.image-item {
  width: calc(50% - 2.5px);
  height: 200px;
  object-fit: cover;
  border-radius: 4px;
}

.game-attributes {
  font-size: 14px;
  color: #555;
  margin-top: 12px;
}

.post-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 15px;
  padding: 10px 0; 
  border-top: 1px solid #ddd;
  border-bottom: 1px solid #ddd;
  height: 30px;
}

.reactions {
  display: flex;
  align-items: center;
  justify-content: space-evenly;
  flex: 1;
}

.reaction {
  font-size: 28px;
  color: #606770;
  cursor: pointer;
  transition: color 0.2s;
}

.reaction:hover {
  color: #1877f2;
}

.post-image {
  width: calc(50% - 2.5px);
  border-radius: 5px;
}
.image-grid {
  display: grid;
  gap: 5px;
  border-radius: 5px;
  overflow: hidden;
}

.image-grid.single-image {
  grid-template-columns: 1fr;
}

.image-grid.two-images {
  grid-template-columns: 1fr 1fr;
}

.image-grid.three-images {
  grid-template-columns: 1fr 1fr;
  grid-template-rows: 1fr 1fr;
}

.image-grid.multiple-images {
  grid-template-columns: repeat(2, 1fr);
  grid-template-rows: repeat(2, 1fr);
}

.image-item {
  width: 100%;
  height: auto;
  object-fit: cover;
  border-radius: 4px;
}
.more-images-overlay {
  position: relative;
  width: calc(50% - 2.5px);
  border-radius: 5px;
  overflow: hidden;
  display: flex;
  justify-content: center;
  align-items: center;
  background: rgba(255, 87, 34, 0.8);
  color: white;
  font-size: 24px;
  font-weight: bold;
}

.post-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 15px;
}

.reactions {
  display: flex;
  gap: 10px;
}

.reaction {
  font-size: 14px;
  color: #555;
}

.liked-by {
  display: flex;
  align-items: center;
  gap: 5px;
}

.liked-avatar {
  width: 30px;
  height: 30px;
  border-radius: 50%;
  border: 1px solid #ddd;
}
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5); /* Semi-transparent black */
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000; /* Ensure it appears on top */
}

/* Modal content: card-like box with padding and rounded corners */
.modal-content {
  background-color: #ffffff; /* White background */
  border-radius: 8px; /* Rounded corners */
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1); /* Soft shadow for depth */
  padding: 24px;
  max-width: 500px;
  width: 90%; /* Responsive width */
  text-align: center;
}

/* Success icon: LinkedIn blue and centered */
.success-icon {
  font-size: 40px;
  color: #0073b1; /* LinkedIn blue color */
  margin-bottom: 16px;
}

/* Heading styling */
.modal-content h2 {
  font-size: 24px;
  font-weight: 600;
  margin: 0 0 10px;
  color: #333333; /* Dark text color */
}

/* Paragraph text styling */
.modal-content p {
  font-size: 16px;
  color: #666666; /* Gray text color */
  margin-bottom: 20px;
}

/* Modal buttons: LinkedIn-style button */
.modal-buttons {
  text-align: center;
}

.close-button {
  background-color: #0073b1; /* LinkedIn blue color */
  color: #ffffff;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.3s;
}

.close-button:hover {
  background-color: #005582; /* Darker blue on hover */
}

.modal-content {
  background-color: white;
  padding: 24px;
  width: 450px;
  max-width: 90%;
  border-radius: 8px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
  font-family: "Helvetica", "Arial", sans-serif;
}

.modal-content h2 {
  margin-bottom: 16px;
  font-size: 20px;
  color: #333;
  text-align: center;
}

.modal-content form div {
  margin-bottom: 12px;
}

.modal-content label {
  font-weight: 600;
  font-size: 14px;
  color: #555;
  margin-bottom: 4px;
  display: block;
}

.modal-content input {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
  color: #333;
  transition: border-color 0.2s;
}

.modal-content input:focus {
  border-color: #0073b1;
  outline: none;
}

.modal-buttons {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 16px;
}

.modal-buttons button {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
}

.modal-buttons button[type="submit"] {
  background-color: #0073b1;
  color: white;
}

.modal-buttons button[type="button"] {
  background-color: #e6e6e6;
  color: #333;
}
.input-container {
  position: relative;
  margin-bottom: 16px;
}

.linkedin-input {
  width: 40%;
  padding: 10px 14px;
  border: 1px solid #dce6f1;
  border-radius: 4px;
  background-color: #fff;
  font-size: 14px;
  color: #333;
  outline: none;
  transition: border-color 0.2s, box-shadow 0.2s;
  box-sizing: border-box;
}

.linkedin-input:focus {
  border-color: #0073b1;
  box-shadow: 0 0 0 1px #0073b1;
}
@media (max-width: 1024px) {
  .content-container {
    flex-direction: column;
    align-items: center;
  }

  .post-container,
  .post-album-container,
  .page-container {
    width: 90%;
    margin-left: 0;
  }
}

@media (max-width: 768px) {
  .content-container {
    flex-direction: column;
    align-items: center;
    gap: 15px;
  }

  .post-container,
  .post-album-container,
  .page-container {
    width: 100%;
    margin-left: 0;
  }

  .post-input {
    font-size: 14px;
    padding: 8px;
  }

  .stats button {
    padding: 8px 15px;
    font-size: 12px;
  }

  .post-button {
    height: 40px;
    font-size: 14px;
  }
}
.post-album-container {
  position: relative; /* Ensures the status badge is positioned correctly */
  background-color: #ffffff;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 16px;
  margin-bottom: 16px;
  max-width: 600px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

/* Status Badge Styling */
.status-badge {
  position: absolute;
  top: 8px;
  right: 8px;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: bold;
  color: white;
}

/* Colors for Status */
.status-selling {
  background-color: #4caf50; /* Green for 'Đang bán' */
}

.status-sold {
  background-color: #f44336; /* Red for 'Đã bán' */
}
.status-list {
  list-style: none;
  padding: 0;
  margin-top: 10px;
}

.status-list li {
  font-size: 14px;
  color: #0073b1;
  cursor: pointer;
  padding: 10px 0;
  border-bottom: 1px solid #ddd;
  transition: color 0.3s;
}

.status-list li:hover,
.status-list li.active {
  color: #005582;
  font-weight: bold;
}

</style>
