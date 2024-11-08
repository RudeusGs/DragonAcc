<template>
    <div class="language-content">
      <h2>Chọn Ngôn Ngữ</h2>
      <ul class="language-list">
        <li @click="$emit('close')">🇺🇸 English</li>
        <li @click="$emit('close')">🇻🇳 Tiếng Việt</li>
        <li @click="$emit('close')">🇨🇳 中文</li>
        <li @click="$emit('close')">🇯🇵 日本語</li>
        <li @click="$emit('close')">🇰🇷 한국어</li>
        <li @click="$emit('close')">🇫🇷 Français</li>
      </ul>
      <button class="close-button" @click="$emit('close')">Đóng</button>
    </div>
  </template>
  
  <style scoped>
  .language-content {
    padding: 20px;
    text-align: center;
  }
  
  .language-list {
    list-style: none;
    padding: 0;
  }
  
  .language-list li {
    margin: 10px 0;
    cursor: pointer;
    font-size: 18px;
  }
  
  .close-button {
    margin-top: 20px;
    padding: 10px 20px;
    background-color: #0073b1;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
  }
  
  .close-button:hover {
    background-color: #005582;
  }
  </style>
  