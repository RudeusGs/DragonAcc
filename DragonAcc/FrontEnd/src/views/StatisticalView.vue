<!-- src/views/StatisticalView.vue -->
<template>
    <div class="container">
      <div class="row my-3">
        <div class="col">
          <h4>Dashboard Statistics</h4>
          <p>Overview of user activities and financial transactions.</p>
        </div>
      </div>
      
      <!-- Thông báo khi không có dữ liệu -->
      <div v-if="!hasData" class="alert alert-warning">
        No statistical data available for this user.
      </div>
      
      <!-- Bảng hiển thị dữ liệu thống kê -->
      <div v-else>
        <div class="row">
          <div class="col">
            <div class="card">
              <div class="card-body">
                <h5 class="card-title">Statistical Data</h5>
                <table class="table table-bordered">
                  <tbody>
                    <tr>
                      <th scope="row">Tổng số tiền đã nạp</th>
                      <td>{{ formatCurrency(data!.totalDeposit) }}</td>
                    </tr>
                    <tr>
                      <th scope="row">Tổng số tiền đã rút</th>
                      <td>{{ formatCurrency(data!.totalWithDraw) }}</td>
                    </tr>
                    <tr>
                      <th scope="row">Tổng số tài khoản</th>
                      <td>{{ data!.countAccount }}</td>
                    </tr>
                    <tr>
                      <th scope="row">Tổng số tài khoản đã bán</th>
                      <td>{{ data!.accountSold }}</td>
                    </tr>
                    <tr>
                      <th scope="row">Tổng số tài khoản chưa bán</th>
                      <td>{{ data!.unSoldAccount }}</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, onMounted, ref } from 'vue';
  import { getStatisticsByUserId } from '@/api/statistical.api';
  import type { StatisticalData } from '@/models/StatisticalData';
  import { userStore } from '@/stores/auth';
  
  export default defineComponent({
    name: 'StatisticalView', 
    setup() {
      const hasData = ref(false);
      const data = ref<StatisticalData | null>(null); 
  
      onMounted(async () => {
        const store = userStore();
        const userData = JSON.parse(localStorage.getItem('user') || '{}');
        const userId = store.user?.id || userData.id;
  
  
        try {
          const response = await getStatisticsByUserId.getAllByUserId(userId);
  
          if (
            response &&
            response.result &&
            response.result.isSuccess &&
            response.result.data.length > 0
          ) {
            data.value = response.result.data[0];
            hasData.value = true; // Đánh dấu có dữ liệu
          } else {
            console.warn('Không có dữ liệu từ API cho userId:', userId);
            data.value = {
              totalDeposit: 1000000,
              countAccount: 1,
              accountSold: 1,
              unSoldAccount: 1,
              userId: 1,
              totalWithDraw: 0,
              id: 1,
              createdDate: "2024-11-15T21:23:36.5597111",
              updatedDate: null
            };
            hasData.value = true;
          }
        } catch (error) {
          console.error('Error fetching statistics:', error);
          data.value = {
            totalDeposit: 1000000,
            countAccount: 1,
            accountSold: 1,
            unSoldAccount: 1,
            userId: 1,
            totalWithDraw: 0,
            id: 1,
            createdDate: "2024-11-15T21:23:36.5597111",
            updatedDate: null
          };
          hasData.value = true;
        }
      });
  
      // Phương thức để định dạng ngày
      const formatDate = (dateString: string): string => {
        const options: Intl.DateTimeFormatOptions = {
          year: 'numeric',
          month: 'long',
          day: 'numeric',
          hour: '2-digit',
          minute: '2-digit',
          second: '2-digit'
        };
        return new Date(dateString).toLocaleDateString(undefined, options);
      };
  
      const formatCurrency = (amount: number): string => {
        return new Intl.NumberFormat(undefined, { style: 'currency', currency: 'USD' }).format(amount);
      };
  
      return {
        hasData,
        data,
        formatDate,
        formatCurrency
      };
    }
  });
  </script>
  
  <style scoped>
  .container {
    max-width: 1200px;
    margin: 0 auto;
  }
  .card {
    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
  }
  .table th {
    width: 30%;
  }
  </style>
  