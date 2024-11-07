<template>
    <div class="auction-detail-container">
      <!-- Auction Item Header -->
      <div class="header-section">
        <h2 class="item-title">{{ auctionItem.title }}</h2>
        <p class="item-category">{{ auctionItem.category }}</p>
      </div>
  
      <!-- Product Details Section -->
      <div class="product-details">
        <div class="image-gallery">
          <img :src="auctionItem.mainImage" alt="Main Image" class="main-image" />
          <div class="thumbnail-gallery">
            <img
              v-for="(image, index) in auctionItem.images"
              :src="image"
              :key="index"
              alt="Thumbnail"
              class="thumbnail"
              @click="selectImage(index)"
            />
          </div>
        </div>
        <div class="info-section">
          <div class="price-info">
            <p class="current-price">
              Giá hiện tại: <span>{{ auctionItem.currentPrice }} VNĐ</span>
            </p>
            <p class="starting-price">
              Giá khởi điểm: <span>{{ auctionItem.startPrice }} VNĐ</span>
            </p>
          </div>
          <div class="time-left">
            Thời gian còn lại: <span>{{ auctionItem.timeLeft }}</span>
          </div>
          <div class="bid-section">
            <label for="bidAmount">Nhập số tiền đấu giá:</label>
            <input
              type="number"
              id="bidAmount"
              v-model="bidAmount"
              class="bid-input"
              placeholder="Nhập số tiền của bạn"
            />
            <button class="btn-place-bid" @click="placeBid">Đặt giá</button>
          </div>
        </div>
      </div>
  
      <!-- Product Description Section -->
      <div class="description-section">
        <h3>Mô tả sản phẩm</h3>
        <p>{{ auctionItem.description }}</p>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  
  const auctionItem = ref({
    title: 'MacBook Pro 2021',
    category: 'Công nghệ',
    mainImage: 'data:image/webp;base64,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',
    images: [
      'https://via.placeholder.com/80',
      'https://via.placeholder.com/80',
      'https://via.placeholder.com/80',
    ],
    currentPrice: '45,000,000',
    startPrice: '40,000,000',
    timeLeft: '3 ngày 12 giờ',
    description:
      'MacBook Pro 2021 với màn hình 14 inch, chip M1 Pro, RAM 16GB, bộ nhớ SSD 512GB. Sản phẩm còn bảo hành 6 tháng, mới 98%.',
  });
  
  const bidAmount = ref<number | null>(null);
  
  const placeBid = () => {
    if (bidAmount.value && bidAmount.value > 0) {
      alert(`Bạn đã đặt giá ${bidAmount.value} VNĐ thành công!`);
    } else {
      alert('Vui lòng nhập số tiền hợp lệ.');
    }
  };
  
  const selectImage = (index: number) => {
    auctionItem.value.mainImage = auctionItem.value.images[index];
  };
  </script>
  
  <style scoped>
  /* Container Styling */
  .auction-detail-container {
    padding: 30px;
    max-width: 1000px;
    margin: 0 auto;
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    color: #333;
    background-color: #f5f8fa;
    border-radius: 10px;
    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  }
  
  /* Header Section */
  .header-section {
    text-align: center;
    margin-bottom: 20px;
  }
  
  .item-title {
    font-size: 26px;
    font-weight: 700;
    color: #0073b1;
  }
  
  .item-category {
    font-size: 14px;
    color: #888;
    margin-top: 5px;
  }
  
  /* Product Details Section */
  .product-details {
    display: flex;
    gap: 20px;
    margin-bottom: 20px;
  }
  
  .image-gallery {
    flex: 1;
  }
  
  .main-image {
    width: 100%;
    border-radius: 8px;
    object-fit: cover;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  }
  
  .thumbnail-gallery {
    display: flex;
    gap: 10px;
    margin-top: 10px;
  }
  
  .thumbnail {
    width: 80px;
    height: 80px;
    border-radius: 8px;
    object-fit: cover;
    cursor: pointer;
    transition: transform 0.3s;
  }
  
  .thumbnail:hover {
    transform: scale(1.05);
  }
  
  .info-section {
    flex: 1;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }
  
  .price-info {
    margin-bottom: 15px;
  }
  
  .current-price,
  .starting-price {
    font-size: 18px;
    font-weight: bold;
    margin: 5px 0;
  }
  
  .time-left {
    font-size: 16px;
    color: #e74c3c;
    font-weight: bold;
    margin-bottom: 15px;
  }
  
  .bid-section {
    display: flex;
    flex-direction: column;
    gap: 10px;
  }
  
  .bid-input {
    padding: 10px;
    border: 1px solid #ddd;
    border-radius: 5px;
    font-size: 14px;
    outline: none;
    transition: border-color 0.3s;
  }
  
  .bid-input:focus {
    border-color: #0073b1;
  }
  
  .btn-place-bid {
    background-color: #0073b1;
    color: #ffffff;
    border: none;
    border-radius: 25px;
    padding: 10px 20px;
    font-size: 14px;
    font-weight: bold;
    cursor: pointer;
    transition: background-color 0.3s, transform 0.2s;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
  }
  
  .btn-place-bid:hover {
    background-color: #005582;
    transform: translateY(-3px);
  }
  
  .btn-place-bid:active {
    background-color: #004161;
    transform: translateY(0);
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2) inset;
  }
  
  /* Description Section */
  .description-section {
    background-color: #ffffff;
    border-radius: 10px;
    padding: 20px;
    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  }
  
  .description-section h3 {
    font-size: 20px;
    font-weight: 600;
    margin-bottom: 10px;
  }
  
  .description-section p {
    font-size: 14px;
    color: #555;
  }
  </style>
  