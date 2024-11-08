<template>
  <LoadingSpinner :isLoading="loading" />
    <div v-if="!loading" class="container-fluid py-5">
      <h1 class="text-center mb-4">Danh sách tài khoản đã mua</h1>
      <div class="table-container">
        <div class="table-responsive">
          <table class="table linkedin-table" v-if="purchasedAccounts.length > 0">
            <thead>
              <tr>
                <th></th>
                <th scope="col">#</th>
                <th scope="col">Username</th>
                <th scope="col">Password</th>
                <th scope="col">Ngày mua</th>
                <th scope="col">Số tiền</th>
                <th scope="col">Báo cáo</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(account, index) in purchasedAccounts" :key="index" class="table-row">
                <td>
                  <img src="../assets/anhdong.gif" alt="User Avatar" class="avatar-img" />
                </td>
                <th scope="row">{{ index + 1 }}</th>
                <td>{{ account.username }}</td>
                <td>{{ account.password }}</td>
                <td>{{ formatDate(account.purchaseDate) }}</td>
                <td>{{ formatPrice(account.price) }}</td>
              </tr>
            </tbody>
          </table>
          <p v-else>Không có tài khoản nào được mua.</p>
        </div>
      </div>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import purchasedaccountApi from '@/api/purchasedaccountservice.api';
  import { userStore } from '@/stores/auth';
  import LoadingSpinner from '@/components/LoadingSpinner.vue';
  interface Account {
    username: string;
    password: string;
    purchaseDate: string;
    price: string;
  }
  
  export default defineComponent({
    setup() {
      const purchasedAccounts = ref<Account[]>([]);
      const store = userStore();
      const userId = store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;
  
      const fetchPurchasedAccounts = async () => {
        try {
          if (!userId) {
            alert("Vui lòng đăng nhập để xem các tài khoản đã mua.");
            return;
          }
          const response = await purchasedaccountApi.getPurchasedAccountsByUser(userId);
          console.log("Full API response:", response);
  
          if (response && response.data.isSuccess) {
            purchasedAccounts.value = response.data.data.map((account: any) => ({
              username: account.accountName,
              password: account.accountPasswordChange,
              purchaseDate: account.createdDate,
              price: account.price,
            }));
          } else {
            console.error('Failed to fetch purchased accounts:', response?.data?.message || 'Unexpected response format');
          }
        } catch (error) {
          console.error('Error fetching purchased accounts:', error);
        }
      };
  
      const formatPrice = (price: string) => {
        const number = parseFloat(price);
        if (isNaN(number)) return '0 VNĐ';
        return number.toLocaleString('vi-VN') + ' VNĐ';
      };
  
      const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return date.toLocaleDateString();
      };
  
      const reportAccount = (accountId: number) => {
        alert(`Báo cáo tài khoản ID: ${accountId}`);
      };
      const loading = ref(true);
      onMounted(() => {
        loading.value = true;
        fetchPurchasedAccounts();
        loading.value = false;
      });
  
      return {
        purchasedAccounts,
        formatPrice,
        formatDate,
        reportAccount,
        loading,
      };
    },
  });
  </script>
  
  <style scoped>
  h1 {
    font-family: "Segoe UI", "Source Sans Pro", Arial, sans-serif;
    font-weight: 600;
    font-size: 2rem;
    color: #333333;
    line-height: 1.2;
    margin-bottom: 20px;
  }
  
  .container-fluid {
    max-width: 1000px;
    margin: auto;
    animation: fadeIn 1s ease-in-out;
  }
  
  @keyframes fadeIn {
    from { opacity: 0; transform: translateY(-20px); }
    to { opacity: 1; transform: translateY(0); }
  }
  
  .table-container {
    position: relative;
    padding: 20px;
    border-radius: 10px;
    background: linear-gradient(145deg, #e6ebf1, #ffffff);
    box-shadow: 8px 8px 15px rgba(0, 0, 0, 0.1), -8px -8px 15px rgba(255, 255, 255, 0.5);
    transition: transform 0.3s, box-shadow 0.3s;
  }
  
  .table-container:hover {
    transform: scale(1.01);
    box-shadow: 10px 10px 20px rgba(0, 0, 0, 0.15), -10px -10px 20px rgba(255, 255, 255, 0.6);
  }
  
  .linkedin-table {
    width: 100%;
    background-color: #fff;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.08);
    animation: slideIn 0.5s ease-out;
  }
  
  @keyframes slideIn {
    from { transform: translateX(-20px); opacity: 0; }
    to { transform: translateX(0); opacity: 1; }
  }
  
  .linkedin-table thead {
    background: linear-gradient(135deg, #f3f6f8, #e3e8ec);
    border-bottom: 2px solid #d1d9e1;
  }
  
  .linkedin-table th,
  .linkedin-table td {
    padding: 15px;
    text-align: left;
    font-size: 0.875rem;
    color: #333;
  }
  
  .linkedin-table th {
    font-weight: 600;
    color: #5e6c84;
  }
  
  .linkedin-table tbody tr {
    transition: background-color 0.3s ease, transform 0.3s ease;
  }
  
  .linkedin-table tbody tr:hover {
    background-color: #eef3f8;
    transform: scale(1.02);
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.08);
  }
  
  .linkedin-table tbody tr:nth-child(even) {
    background-color: #fafbfc;
  }
  
  .avatar-img {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    transition: transform 0.3s ease;
    box-shadow: 0px 2px 8px rgba(0, 0, 0, 0.2);
  }
  
  .avatar-img:hover {
    transform: scale(1.2);
  }
  
  .action-btn {
    padding: 6px 12px;
    font-size: 0.75rem;
    color: #0073b1;
    background-color: #eaf3fa;
    border: 1px solid #0073b1;
    border-radius: 4px;
    cursor: pointer;
    transition: background-color 0.2s, color 0.2s;
  }
  
  .action-btn.report {
    color: #d93025;
    background-color: #fdeceb;
    border: 1px solid #d93025;
    animation: pulse 1s infinite alternate;
  }
  
  @keyframes pulse {
    from { transform: scale(1); }
    to { transform: scale(1.05); }
  }
  
  .action-btn:hover {
    background-color: #cfe0f5;
    color: #005682;
  }
  
  .action-btn.report:hover {
    background-color: #fbcac8;
    color: #b31814;
  }
  
  @media (max-width: 768px) {
    .linkedin-table th, .linkedin-table td {
      padding: 10px;
      font-size: 0.75rem;
    }
  
    .action-btn {
      font-size: 0.7rem;
    }
  }
  </style>
  