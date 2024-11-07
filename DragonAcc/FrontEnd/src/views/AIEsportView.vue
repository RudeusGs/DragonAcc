<template>
    <div class="development-container">
      <img style="width: 300px;" src="../assets/hinhdong.gif" alt="Loading Bunny" class="bunny-icon" />
      <h1 class="development-title">Đang trong giai đoạn phát triển</h1>
      <p class="development-description">Chúng tôi đang làm việc chăm chỉ để mang đến cho bạn trải nghiệm tốt nhất. Hãy quay lại sau nhé!</p>
      <div class="development-loader">
        <div class="loader-circle"></div>
      </div>
    </div>
  </template>
  
  <style scoped>
  /* Main Container */
  .development-container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100vh;
    text-align: center;
    background: linear-gradient(135deg, #ece9e6, #ffffff);
    font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
    color: #333;
    padding: 20px;
  }
  
  /* Icon Styling */
  .development-icon {
    font-size: 80px;
    color: #ff8c00;
    margin-bottom: 20px;
    animation: rotate 3s linear infinite;
  }
  
  /* Title Styling */
  .development-title {
    font-size: 36px;
    font-weight: bold;
    color: #444;
    margin-bottom: 10px;
    text-shadow: 1px 1px 3px rgba(0, 0, 0, 0.1);
  }
  
  /* Description Styling */
  .development-description {
    font-size: 18px;
    color: #666;
    margin-bottom: 30px;
    max-width: 600px;
  }
  
  /* Loader Animation */
  .development-loader {
    display: flex;
    justify-content: center;
    align-items: center;
  }
  
  .loader-circle {
    width: 50px;
    height: 50px;
    border: 5px solid #ddd;
    border-top: 5px solid #ff8c00;
    border-radius: 50%;
    animation: spin 1s linear infinite;
  }
  
  /* Animations */
  @keyframes spin {
    0% {
      transform: rotate(0deg);
    }
    100% {
      transform: rotate(360deg);
    }
  }
  
  @keyframes rotate {
    0% {
      transform: rotate(0deg);
    }
    100% {
      transform: rotate(360deg);
    }
  }
  </style>
  