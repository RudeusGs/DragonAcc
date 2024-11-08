<template>
    <div class="approval-container mt-5">
      <h2>Danh sách thẻ cào đang chờ</h2>
      <table class="approval-table">
        <thead>
          <tr>
            <th>Nhà cung cấp</th>
            <th>Số thẻ</th>
            <th>Số seri</th>
            <th>Mệnh giá</th>
            <th>Trạng thái</th>
            <th>Duyệt</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="card in cards" :key="card.numberCard">
            <td>{{ card.telecomO }}</td>
            <td>{{ card.numberCard }}</td>
            <td>{{ card.numberSeri }}</td>
            <td>{{ card.depositAmount ? card.depositAmount.toLocaleString() : '0' }} VND</td>
            <td>{{ card.status }}</td>
            <td>
              <button
                class="approve-btn"
                :disabled="card.status === 'Thành công'"
                @click="showModal(card)"
              >
                Approve
              </button>
            </td>
          </tr>
        </tbody>
      </table>
  
      <!-- Confirmation Modal -->
      <div v-if="isModalVisible" class="modal-overlay">
        <div class="modal-content">
          <h3>Xác nhận duyệt thẻ cào</h3>
          <p>Bạn có chắc chắn muốn duyệt thẻ cào này không?</p>
          <div class="modal-actions">
            <button class="modal-btn cancel-btn" @click="closeModal">Hủy</button>
            <button class="modal-btn confirm-btn" @click="confirmApproval">Duyệt</button>
          </div>
        </div>
      </div>
  
      <!-- Success Modal -->
      <div v-if="isSuccessModalVisible" class="modal-overlay">
        <div class="modal-content">
          <h3>Duyệt thành công!</h3>
          <p>Thẻ cào đã được duyệt thành công.</p>
          <button class="modal-btn confirm-btn" @click="closeSuccessModal">Đóng</button>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import api from '@/api/deposit.api';
  
  export default {
    name: "CardApprovalTable",
    data() {
      return {
        cards: [],
        isModalVisible: false,
        isSuccessModalVisible: false,
        selectedCardId: null,
      };
    },
    async created() {
      await this.fetchCards();
    },
    methods: {
      async fetchCards() {
        try {
          const response = await api.getAll();
          this.cards = response.data.result?.data || [];
        } catch (error) {
          console.error("Error fetching cards:", error);
        }
      },
      showModal(card) {
        this.selectedCardId = card.id;
        this.isModalVisible = true;
      },
      closeModal() {
        this.isModalVisible = false;
        this.selectedCardId = null;
      },
      closeSuccessModal() {
        this.isSuccessModalVisible = false;
      },
      async confirmApproval() {
        if (!this.selectedCardId) return;
        try {
          await api.updateStatus(this.selectedCardId);
          await this.fetchCards();
          this.isModalVisible = false;
          this.isSuccessModalVisible = true;
        } catch (error) {
          console.error("Error approving card:", error);
          alert("Failed to approve the card. Please try again.");
        } finally {
          this.closeModal();
        }
      },
    },
  };
  </script>
  
  <style scoped>
  .approval-container {
    max-width: 1000px;
    margin: auto;
    background-color: #ffffff;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    font-family: Arial, sans-serif;
  }
  
  h2 {
    color: #0a66c2;
    text-align: center;
    font-size: 24px;
    margin-bottom: 20px;
  }
  
  .approval-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 14px;
  }
  
  .approval-table th,
  .approval-table td {
    padding: 12px;
    text-align: left;
    border-bottom: 1px solid #ddd;
  }
  
  .approval-table th {
    color: #555555;
    font-weight: bold;
  }
  
  .approval-table td {
    color: #333333;
  }
  
  .approve-btn {
    background-color: #0a66c2;
    color: white;
    border: none;
    border-radius: 4px;
    padding: 6px 12px;
    font-size: 14px;
    cursor: pointer;
    transition: background-color 0.3s;
  }
  
  .approve-btn:disabled {
    background-color: #ccc;
    cursor: not-allowed;
  }
  
  .approve-btn:hover:not(:disabled) {
    background-color: #004182;
  }
  
  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
  }
  
  .modal-content {
    background-color: white;
    padding: 20px;
    border-radius: 8px;
    width: 300px;
    text-align: center;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  }
  
  .modal-content h3 {
    color: #0a66c2;
    margin-bottom: 15px;
  }
  
  .modal-actions {
    display: flex;
    justify-content: space-between;
    margin-top: 20px;
  }
  
  .modal-btn {
    padding: 8px 16px;
    font-size: 14px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
  }
  
  .cancel-btn {
    background-color: #e0e0e0;
    color: #333;
  }
  
  .confirm-btn {
    background-color: #0a66c2;
    color: white;
  }
  
  .confirm-btn:hover {
    background-color: #004182;
  }
  </style>
  