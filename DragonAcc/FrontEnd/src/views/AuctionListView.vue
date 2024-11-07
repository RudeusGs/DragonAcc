<template>
    <div class="auction-container">
      <h2 class="page-title">Danh Sách Đấu Giá</h2>
      <div class="auction-list">
        <!-- Auction Item Card -->
        <div v-for="(item, index) in auctionItems" :key="index" class="auction-card">
          <div class="item-header">
            <img :src="item.image" alt="Item Image" class="item-image" />
            <div class="item-info">
              <h3 class="item-title">{{ item.title }}</h3>
              <p class="item-description">{{ item.description }}</p>
            </div>
          </div>
          <div class="item-details">
            <div class="detail">
              <span class="detail-label">Giá khởi điểm:</span>
              <span class="detail-value">{{ item.startPrice }} VNĐ</span>
            </div>
            <div class="detail">
              <span class="detail-label">Giá hiện tại:</span>
              <span class="detail-value">{{ item.currentPrice }} VNĐ</span>
            </div>
            <div class="detail">
              <span class="detail-label">Thời gian còn lại:</span>
              <span class="detail-value">{{ item.timeLeft }}</span>
            </div>
          </div>
          <div class="item-actions">
            <button class="btn-bid" ><a href="/auctiondetail">Đấu giá ngay</a></button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  
  const auctionItems = ref([
    {
      id: 1,
      image: 'data:image/webp;base64,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',
      title: 'Laptop Gaming Acer',
      description: 'Laptop gaming mạnh mẽ với CPU Intel i7, GTX 1660 Ti.',
      startPrice: '10,000,000',
      currentPrice: '15,500,000',
      timeLeft: '2 giờ 30 phút',
    },
    {
      id: 2,
      image: 'data:image/jpeg;base64,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',
      title: 'iPhone 13 Pro Max',
      description: 'iPhone 13 Pro Max chính hãng, màu xanh Sierra Blue.',
      startPrice: '20,000,000',
      currentPrice: '25,000,000',
      timeLeft: '1 ngày 5 giờ',
    },
    // Add more items as needed
  ]);
  
  const placeBid = (id: number) => {
    alert(`Bạn đã đặt giá thành công cho sản phẩm có ID: ${id}`);
  };
  </script>
  
  <style scoped>
  /* Container Styling */
  .auction-container {
    padding: 30px;
    max-width: 1200px;
    margin: 0 auto;
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    color: #333;
    background-color: #f5f8fa;
  }
  
  .page-title {
    font-size: 26px;
    font-weight: 700;
    color: #0073b1;
    text-align: center;
    margin-bottom: 25px;
    animation: slideIn 0.6s ease-out;
  }
  
  /* Auction List Styling */
  .auction-list {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 20px;
  }
  
  /* Auction Item Card Styling */
  .auction-card {
    background-color: #ffffff;
    border-radius: 10px;
    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
    transition: transform 0.3s, box-shadow 0.3s;
    overflow: hidden;
  }
  
  .auction-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
  }
  
  .item-header {
    display: flex;
    align-items: center;
    padding: 15px;
  }
  
  .item-image {
    width: 80px;
    height: 80px;
    border-radius: 8px;
    object-fit: cover;
    margin-right: 15px;
  }
  
  .item-info {
    flex: 1;
  }
  
  .item-title {
    font-size: 18px;
    font-weight: 600;
    color: #0073b1;
    margin: 0;
  }
  
  .item-description {
    font-size: 14px;
    color: #666;
    margin-top: 5px;
  }
  
  /* Item Details Styling */
  .item-details {
    border-top: 1px solid #f0f0f0;
    padding: 15px;
    display: flex;
    justify-content: space-between;
  }
  
  .detail {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
  }
  
  .detail-label {
    font-size: 12px;
    color: #888;
    margin-bottom: 2px;
  }
  
  .detail-value {
    font-size: 14px;
    font-weight: 600;
    color: #333;
  }
  
  /* Action Button Styling */
  .item-actions {
    border-top: 1px solid #f0f0f0;
    padding: 15px;
    text-align: center;
  }
  
  .btn-bid {
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
  
  .btn-bid:hover {
    background-color: #005582;
    transform: translateY(-3px);
  }
  
  .btn-bid:active {
    background-color: #004161;
    transform: translateY(0);
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2) inset;
  }
  
  /* Animations */
  @keyframes slideIn {
    from {
      transform: translateY(-20px);
      opacity: 0;
    }
    to {
      transform: translateY(0);
      opacity: 1;
    }
  }
  
  /* Responsive Design */
  @media (max-width: 768px) {
    .item-header {
      flex-direction: column;
      align-items: flex-start;
    }
  
    .item-image {
      margin-bottom: 10px;
    }
  
    .item-details {
      flex-direction: column;
      gap: 10px;
    }
  }
  </style>
  