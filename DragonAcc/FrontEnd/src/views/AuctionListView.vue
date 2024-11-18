<template>
  <div class="auction-list container mt-5 mb-5">
    <h2 class="text-center mb-4">Danh sách đấu giá</h2>

    <div v-if="loading" class="text-center">
      <p>Đang tải các phiên đấu giá...</p>
    </div>

    <div v-if="errorMessage" class="alert alert-danger">{{ errorMessage }}</div>

    <div v-if="auctions.length > 0" class="row g-4">
      <div
        class="col-md-6 col-lg-4"
        v-for="(auction, index) in auctions"
        :key="auction.id"
      >
        <div class="auction-card card shadow-sm">
          <img
            :src="getFullImageUrl(auction.image)"
            class="card-img-top"
            alt="Hình ảnh đấu giá"
            @load="imageLoaded(index)"
            @error="handleImageError(index)"
            v-if="!imageError[index]"
          />
          <div v-else class="text-center">
            <img src="https://via.placeholder.com/336x198?text=No+Image" alt="No Image" />
          </div>
          <div class="card-body">
            <h5 class="card-title">{{ auction.auctionName }}</h5>

            <!-- Hiển thị giá -->
            <p v-if="getAuctionStatus(auction) === 'upcoming'" class="card-text">
              Giá bắt đầu: {{ auction.startPrice }} VNĐ
            </p>
            <p v-else-if="getAuctionStatus(auction) === 'ongoing'" class="card-text">
              Giá hiện tại: {{ auction.currentPrice }} VNĐ
            </p>
            <p v-else-if="getAuctionStatus(auction) === 'ended'" class="card-text">
              Giá cuối: {{ auction.currentPrice }} VNĐ
            </p>

            <!-- Hiển thị người chiến thắng nếu đấu giá đã kết thúc -->
            <!-- <div v-if="getAuctionStatus(auction) === 'ended'" class="card-text">
              <p>Người chiến thắng ID: {{ auction.winnerId || "Chưa có" }}</p>  
            </div> -->

            <p class="card-text">Ngày bắt đầu: {{ formatDate(auction.startDateTime) }}</p>

            <!-- Thời gian còn lại -->
            <!-- <p v-if="getAuctionStatus(auction) === 'ongoing'" class="card-text">
              Thời gian còn lại: {{ getRemainingTime(auction) }}
            </p> -->

            <!-- Nút hành động -->
            <button
              :class="buttonClass(auction)"
              class="btn w-100 mt-3"
              :disabled="getAuctionStatus(auction) !== 'ongoing'"
              @click="goToAuctionDetail(auction.id)"
            >
              {{ buttonLabel(auction) }}
            </button>
          </div>
        </div>
      </div>
    </div>
    <div v-else class="alert alert-info">Hiện tại không có phiên đấu giá nào.</div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, onBeforeUnmount } from "vue";
import type { AuctionModel } from "@/models/auctionmodel";
import AuctionApi from "@/api/auction.api";
import { useRouter } from "vue-router";

export default defineComponent({
  name: "AuctionList",
  setup() {
    const auctions = ref<AuctionModel[]>([]);
    const loading = ref(true);
    const errorMessage = ref<string | null>(null);
    const router = useRouter();

    // Function to determine auction status
    const getAuctionStatus = (auction: AuctionModel): string => {
      if (!auction.status) return "ended"; // Auction has ended

      const now = new Date();
      const startDate = new Date(auction.startDateTime);

      // Log the dates for debugging
      console.log(`Auction ID: ${auction.id}, Now: ${now}, StartDate: ${startDate}`);
      console.log(now<startDate);
      if (now < startDate) return "upcoming"; // Auction is upcoming

      // Calculate the auction end time
      const durationMs = parseAuctionDuration(auction.timeAuction);
      const endDate = new Date(startDate.getTime() + durationMs);

      if (!auction.status) {
        return "ended"; // Auction has ended
      }

      return "ongoing"; // Auction is ongoing
    };

    // Function to parse auction duration from "hh:mm:ss" format
    const parseAuctionDuration = (duration: string): number => {
      const [hours, minutes, seconds] = duration.split(":").map(Number);
      return (hours * 60 * 60 + minutes * 60 + seconds) * 1000;
    };

    // Function to format date including time
    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString("vi-VN", {
        year: "numeric",
        month: "long",
        day: "numeric",
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit",
      });
    };

    // Function to fetch auctions
    const fetchAuctions = async () => {
      try {
        const response = await AuctionApi.getAllAuction();

        if (response.data?.result?.data?.length > 0) {
          auctions.value = response.data.result.data;
        } else {
          errorMessage.value = "Không tìm thấy phiên đấu giá nào.";
        }
      } catch (error) {
        console.error("Lỗi khi tải dữ liệu phiên đấu giá:", error);
        errorMessage.value = "Không thể tải dữ liệu phiên đấu giá. Vui lòng thử lại sau.";
      } finally {
        loading.value = false;
      }
    };

    // Image error handling
    const imageError = ref<boolean[]>([]);

    const imageLoaded = (index: number) => {};

    const handleImageError = (index: number) => {
      imageError.value[index] = true;
    };

    // Timer for countdown
    const timer = ref<number>(Date.now());
    let intervalId: number;

    const getRemainingTime = (auction: AuctionModel): string => {
      timer.value;

      const now = new Date();
      const startDate = new Date(auction.startDateTime);
      const durationMs = parseAuctionDuration(auction.timeAuction);
      const endDate = new Date(startDate.getTime() + durationMs);
      const remainingTimeMs = endDate.getTime() - now.getTime();

      if (remainingTimeMs <= 0) {
        return "Đã kết thúc";
      }

      const hours = Math.floor(remainingTimeMs / (1000 * 60 * 60));
      const minutes = Math.floor((remainingTimeMs % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((remainingTimeMs % (1000 * 60)) / 1000);

      return `${hours} giờ ${minutes} phút ${seconds} giây`;
    };

    const goToAuctionDetail = (id: number) => {
      router.push({ name: "auctionDetail", params: { id } });
    };

    // Button styling based on auction status
    const buttonClass = (auction: AuctionModel) => {
      const status = getAuctionStatus(auction);
      if (status === "ongoing") return "btn-success";
      if (status === "upcoming") return "btn-warning";
      return "btn-secondary"; // For ended auctions
    };

    const buttonLabel = (auction: AuctionModel) => {
      const status = getAuctionStatus(auction);
      if (status === "ongoing") return "Tham gia";
      if (status === "upcoming") return "Sắp tới";
      return "Đã kết thúc";
    };

    // Function to get full image URL
    const getFullImageUrl = (imagePath: string | null) => {
      if (!imagePath) {
        return "https://via.placeholder.com/550x500";
      }
      const baseUrl = "https://localhost:7071/";
      const firstImage = imagePath.split(",")[0];
      return `${baseUrl}${firstImage}`;
    };

    onMounted(() => {
      intervalId = setInterval(() => {
        timer.value = Date.now();
      }, 1000);

      fetchAuctions();
    });

    onBeforeUnmount(() => {
      clearInterval(intervalId);
    });

    return {
      auctions,
      loading,
      errorMessage,
      formatDate,
      buttonClass,
      buttonLabel,
      getAuctionStatus,
      imageError,
      imageLoaded,
      getFullImageUrl,
      handleImageError,
      getRemainingTime,
      goToAuctionDetail,
    };
  },
});
</script>

<style scoped>
/* Your CSS remains the same */
.auction-list {
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 8px;
}

.auction-card {
  transition: all 0.3s ease;
  border-radius: 10px;
  padding: 10px;
  background-color: #fff;
  min-height: 500px;
}

.auction-card:hover {
  transform: translateY(-5px);
  box-shadow: 0px 10px 20px rgba(0, 0, 0, 0.1);
}

.card-img-top {
  height: 200px;
  object-fit: cover;
  border-radius: 10px;
  margin: 10px 0;
  background-color: #f0f0f0;
}

.btn {
  border-radius: 50px;
}

.alert-info,
.alert-danger {
  text-align: center;
  margin-top: 20px;
}
</style>
