<template>
    <div class="approval-container mt-5">
      <h2>Danh sách yêu cầu rút tiền</h2>
      <table class="approval-table">
        <thead>
          <tr>
            <th>#</th>
            <th>Số Tài Khoản Ngân Hàng</th>
            <th>Tên Ngân Hàng</th>
            <th>Số Tiền</th>
            <th>Trạng Thái</th>
            <th>Duyệt</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(withdraw, index) in sortedWithdrawals" :key="withdraw.id">
            <td>{{ index + 1 }}</td>
            <td>{{ withdraw.numberBank }}</td>
            <td>{{ withdraw.nameBank }}</td>
            <td>{{ withdraw.amount ? withdraw.amount.toLocaleString() : '0' }} VND</td>
            <td :class="statusClass(withdraw.status)">{{ withdraw.status }}</td>
            <td>
              <button
                class="approve-btn"
                :disabled="withdraw.status !== 'Chờ duyệt'"
                @click="showModal(withdraw)"
              >
                Duyệt
              </button>
            </td>
          </tr>
        </tbody>
      </table>
  
      <!-- Confirmation Modal -->
      <transition name="modal">
        <div v-if="isModalVisible" class="modal-overlay" @click.self="closeModal">
          <div class="modal-content">
            <h3>Xác nhận duyệt yêu cầu rút tiền</h3>
            <p>Bạn có chắc chắn muốn duyệt yêu cầu rút tiền này không?</p>
            <div class="modal-actions">
              <button class="modal-btn cancel-btn" @click="closeModal">Hủy</button>
              <button class="modal-btn confirm-btn" @click="confirmApproval">Duyệt</button>
            </div>
          </div>
        </div>
      </transition>
  
      <!-- Success Modal -->
      <transition name="modal">
        <div v-if="isSuccessModalVisible" class="modal-overlay" @click.self="closeSuccessModal">
          <div class="modal-content success-modal">
            <h3>Duyệt thành công!</h3>
            <p>Yêu cầu rút tiền đã được duyệt thành công.</p>
            <button class="modal-btn confirm-btn" @click="closeSuccessModal">Đóng</button>
          </div>
        </div>
      </transition>
    </div>
  </template>
  
  <script>
  import { getAll, updateWithdrawStatus } from '@/api/withdrawmoney.api';
  
  export default {
    name: "WithdrawApprovalTable",
    data() {
      return {
        withdrawals: [],
        isModalVisible: false,
        isSuccessModalVisible: false,
        selectedWithdrawId: null,
      };
    },
    computed: {
      sortedWithdrawals() {
        return this.withdrawals.slice().sort((a, b) => {
          if (a.status === 'Chờ duyệt' && b.status !== 'Chờ duyệt') {
            return -1;
          }
          if (a.status !== 'Chờ duyệt' && b.status === 'Chờ duyệt') {
            return 1;
          }
          return 0;
        });
      },
    },
    methods: {
      async fetchWithdrawals() {
        try {
          const response = await getAll();
          this.withdrawals = response.result?.data || [];
        } catch (error) {
          console.error("Error fetching withdrawals:", error);
          this.$toast.error("Không thể tải danh sách rút tiền. Vui lòng thử lại sau.");
        }
      },
      showModal(withdraw) {
        this.selectedWithdrawId = withdraw.id;
        this.isModalVisible = true;
      },
      closeModal() {
        this.isModalVisible = false;
        this.selectedWithdrawId = null;
      },
      closeSuccessModal() {
        this.isSuccessModalVisible = false;
      },
      async confirmApproval() {
        if (!this.selectedWithdrawId) return;
        try {
          await updateWithdrawStatus(this.selectedWithdrawId);
          await this.fetchWithdrawals();
          this.isModalVisible = false;
          this.isSuccessModalVisible = true;
          this.$toast.success("Yêu cầu rút tiền đã được duyệt thành công.");
        } catch (error) {
          console.error("Error approving withdrawal:", error);
          this.$toast.error("Không thể duyệt yêu cầu rút tiền. Vui lòng thử lại.");
        }
      },
      statusClass(status) {
        switch (status) {
          case 'Chờ duyệt':
            return 'status-pending';
          case 'Đã duyệt':
            return 'status-approved';
          case 'Thất bại':
            return 'status-failed';
          default:
            return '';
        }
      },
    },
    mounted() {
      this.fetchWithdrawals();
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
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
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
  
  .approval-table th:first-child,
  .approval-table td:first-child {
    width: 50px;
    text-align: center;
  }
  
  .approval-table th {
    color: #555555;
    font-weight: bold;
  }
  
  .approval-table td {
    color: #333333;
  }
  
  .status-pending {
    color: #ff9800;
    font-weight: bold;
  }
  
  .status-approved {
    color: #4caf50;
    font-weight: bold;
  }
  
  .status-failed {
    color: #f44336;
    font-weight: bold;
  }
  
  .approve-btn {
    background-color: #0a66c2;
    color: white;
    border: none;
    border-radius: 4px;
    padding: 8px 16px;
    font-size: 14px;
    cursor: pointer;
    transition: background-color 0.3s, transform 0.2s;
  }
  
  .approve-btn:disabled {
    background-color: #ccc;
    cursor: not-allowed;
  }
  
  .approve-btn:hover:not(:disabled) {
    background-color: #004182;
    transform: translateY(-2px);
  }
  
  /* Modal Styles */
  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background-color: rgba(0, 0, 0, 0.6);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
    animation: fadeIn 0.3s ease;
  }
  
  .modal-content {
    background-color: white;
    padding: 30px 20px;
    border-radius: 10px;
    width: 350px;
    text-align: center;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
    position: relative;
    animation: slideDown 0.3s ease;
  }
  
  .success-modal {
    border-top: 4px solid #28a745;
  }
  
  .modal-content h3 {
    color: #0a66c2;
    margin-bottom: 15px;
    font-size: 20px;
  }
  
  .modal-content p {
    color: #555555;
    font-size: 16px;
  }
  
  .modal-actions {
    display: flex;
    justify-content: space-between;
    margin-top: 25px;
  }
  
  .modal-btn {
    padding: 10px 20px;
    font-size: 14px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.3s, transform 0.2s;
  }
  
  .cancel-btn {
    background-color: #e0e0e0;
    color: #333;
  }
  
  .cancel-btn:hover {
    background-color: #cfcfcf;
    transform: translateY(-2px);
  }
  
  .confirm-btn {
    background-color: #0a66c2;
    color: white;
  }
  
  .confirm-btn:hover {
    background-color: #004182;
    transform: translateY(-2px);
  }
  
  /* Animations */
  @keyframes fadeIn {
    from {
      opacity: 0;
    }
    to {
      opacity: 1;
    }
  }
  
  @keyframes slideDown {
    from {
      transform: translateY(-20px);
    }
    to {
      transform: translateY(0);
    }
  }
  
  /* Transition for Modal */
  .modal-enter-active,
  .modal-leave-active {
    transition: opacity 0.3s;
  }
  .modal-enter-from,
  .modal-leave-to {
    opacity: 0;
  }
  
  /* Responsive Design */
  @media (max-width: 768px) {
    .modal-content {
      width: 90%;
      padding: 20px 15px;
    }
  
    .approve-btn {
      padding: 6px 12px;
      font-size: 12px;
    }
  
    .modal-btn {
      padding: 8px 14px;
      font-size: 12px;
    }
  
    .approval-table th:first-child,
    .approval-table td:first-child {
      width: 40px;
    }
  }
  </style>
  