<template>
  <LoadingSpinner :isLoading="loading" />
  <div v-if="!loading" class="user-profile-container">
    <!-- Cover Photo -->
    <div class="cover-photo">
      <img src="../assets/anhbia.jpg" alt="Cover Photo" class="cover-image" />
    </div>

    <!-- Profile Header -->
    <div class="profile-header">
      <img
        src="https://tintuc.dienthoaigiakho.vn/wp-content/uploads/2024/01/avatar-nam-nu-trang-2.jpg"
        alt="User Image"
        class="profile-image"
      />
      <div class="user-info">
        <h2 class="user-name"><a style="text-decoration: none;" href="/profileview">{{ userModel.fullName || 'Tên chưa được cập nhật' }}</a></h2>
        <p class="user-email">
          <i class="fas fa-envelope"></i> 
          {{ userModel.email }}
        </p>
        <p class="user-balance">
          <i class="fas fa-money-bill-wave"></i>
          Số dư: {{ formatBalance(userModel.balance) }}
        </p>
        <p class="user-coin">
          <i class="fas fa-coins"></i>
          Số xu: {{ userModel.coin || 0 }}
        </p>
        
      </div>
    </div>
  </div>
</template>


<script setup lang="ts">
import LoadingSpinner from '@/components/LoadingSpinner.vue';
import { ref, onMounted } from 'vue';
import profile from '@/api/profile.api';
import { userStore } from '@/stores/auth';

const loading = ref(true);
const userModel = ref({
  fullName: '',
  email: '',
  balance: '0',
  coin: 0,
  createdDate: null,
});
const formatBalance = (balanceString: string) => {
  const balance = parseFloat(balanceString);
  if (isNaN(balance)) return '0 VNĐ';
  return balance.toLocaleString('vi-VN') + ' VNĐ';
};

const store = userStore();
const userId = store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;

const fetchUserProfile = async () => {
  try {
    if (userId) {
      const response = await profile.getByIdProfile(userId);
      if (response && response.data && response.data.result.isSuccess) {
        const userData = response.data.result.data;
        userModel.value = {
          fullName: userData.fullName || '',
          email: userData.email || '',
          balance: userData.balance || '0',
          coin: userData.coin || 0,
          createdDate: userData.createdDate || '',
        };
      }
    } else {
      console.error('User ID not found.');
    }
  } catch (error) {
    console.error('Error fetching user profile:', error);
  }
};


const fetchData = async () => {
  loading.value = true;
  await fetchUserProfile();
  loading.value = false;
};

onMounted(fetchData);

</script>

<style scoped>
.user-profile-container {
  background-color: #ffffff;
  border-radius: 10px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  padding: 0;
  max-width: calc(100% - 80px);
  margin: 0 auto;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.cover-photo {
  width: 100%;
  height: 250px;
  overflow: hidden;
  border-top-left-radius: 10px;
  border-top-right-radius: 10px;
  
}

.cover-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.profile-header {
  display: flex;
  align-items: center;
  gap: 20px;
  padding: 20px;
  border-bottom: 1px solid #e0e0e0;
  margin-bottom: 20px;
}

.profile-image {
  width: 150px;
  height: 150px;
  border-radius: 50%;
  object-fit: cover;
  border: 2px solid #0073b1;
  margin-top: -75px;
  background-color: #ffffff;
}

.user-info {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.user-name {
  font-size: 10px;
  color: #333;
  font-weight: bold;
}
.user-name1 {
  font-weight: bold;
  color: #0a66c2;
}
.user-email {
  font-size: 16px;
  color: #777;
}

.user-balance,
.user-coin {
  font-size: 14px;
  color: #555;
}

.user-posts-album {
  padding: 20px;
  display: flex; /* Thêm */
  flex-direction: column; /* Thêm */
  align-items: center; /* Thêm */
}

.user-profile-container {
  background-color: #ffffff;
  border-radius: 10px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  padding: 0;
  max-width: 1000px;
  margin: 0 auto;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.cover-photo {
  width: 100%;
  height: 250px;
  overflow: hidden;
  border-top-left-radius: 10px;
  border-top-right-radius: 10px;
}

.cover-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.profile-header {
  display: flex;
  align-items: center;
  gap: 20px;
  padding: 20px;
  border-bottom: 1px solid #e0e0e0;
  margin-bottom: 20px;
}

.profile-image {
  width: 150px;
  height: 150px;
  border-radius: 50%;
  object-fit: cover;
  border: 2px solid #0073b1;
  margin-top: -75px;
  background-color: #ffffff;
}

.user-info {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.user-name {
  font-size: 24px;
  font-weight: 700;
  color: #333;
}

.user-email {
  font-size: 16px;
  color: #777;
}

.user-balance,
.user-coin {
  font-size: 14px;
  color: #555;
}

.user-posts-album {
  padding: 20px;
}

.post-album-container {
  position: relative;
  background-color: #ffffff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  margin-bottom: 20px;
  padding: 15px;
  max-width: 560px;
}

.post-header {
  display: flex;
  align-items: center;
  margin-bottom: 15px;
}

.avatar {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  margin-right: 12px;
}

.post-user-info {
  font-size: 14px;
  color: #333;
}

.post-date {
  font-size: 12px;
  color: #888;
}

.post-content {
  font-size: 16px;
  color: #333;
  margin-bottom: 15px;
}

.post-images {
  margin-top: 10px;
}

.image-grid {
  display: grid;
  gap: 5px;
  grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
  border-radius: 5px;
}

.image-item {
  width: 100%;
  height: auto;
  border-radius: 5px;
  object-fit: cover;
}

.game-attributes {
  font-size: 14px;
  color: #555;
  margin-top: 10px;
}

.price-item {
  display: flex;
  justify-content: center;
  font-weight: bold;
  color: #0073b1;
}

.no-posts-message {
  text-align: center;
  font-size: 16px;
  color: #888;
  margin-top: 20px;
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
  font-size: 14px;
  color: #555;
  cursor: pointer;
  transition: color 0.2s;
}

.reaction:hover {
  color: #0073b1; /* Color change on hover */
}
.image-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr); /* Two columns */
  gap: 10px; /* Space between images */
}

.image-item {
  width: 100%; /* Full width for each item */
  height: auto; /* Maintain aspect ratio */
  border-radius: 8px; /* Rounded corners */
  object-fit: cover; /* Cover the area */
}

.single-image {
  grid-column: span 2; /* Make the single image span across both columns */
  max-height: 300px; /* Set a maximum height for the image */
  object-fit: contain; /* Maintain the aspect ratio and fit within the defined height */
}
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

/* Colors for Statuses */
.status-selling {
  background-color: #4caf50; /* Green for 'Đang bán' */
}

.status-sold {
  background-color: #f44336; /* Red for 'Đã bán' */
}

.status-pending {
  background-color: #ff9800; /* Yellow for 'Đang chờ duyệt' */
}


</style>
