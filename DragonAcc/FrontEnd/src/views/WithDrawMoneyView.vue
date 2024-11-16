<template>
    <div class="withdraw-money-form mt-5">
      <h4 class="title">Yêu Cầu Rút Tiền</h4>
  
      <div v-if="errorMessage" class="alert alert-danger">{{ errorMessage }}</div>
  
      <form @submit.prevent="handleSubmit">
        <!-- Tên Ngân Hàng -->
        <div class="form-group">
          <label for="nameBank">Tên Ngân Hàng</label>
          <div class="input-group">
            <i class="fas fa-building"></i>
            <select v-model="form.nameBank" id="nameBank" class="form-control" required>
              <option value="" disabled selected>Chọn ngân hàng</option>
              <option v-for="bank in banks" :key="bank.id" :value="bank.name">
                <img :src="bank.logo" alt="logo" class="bank-logo" /> {{ bank.name }}
              </option>
            </select>
          </div>
        </div>
  
        <!-- Số Tài Khoản Ngân Hàng -->
        <div class="form-group">
          <label for="numberBank">Số Tài Khoản Ngân Hàng</label>
          <div class="input-group">
            <i class="fas fa-credit-card"></i>
            <input v-model="form.numberBank" type="text" id="numberBank" class="form-control" placeholder="Số tài khoản ngân hàng" required />
          </div>
        </div>
  
        <!-- Số Tiền Rút -->
        <div class="form-group">
          <label for="amount">Số Tiền Rút</label>
          <div class="input-group">
            <i class="fas fa-dollar-sign"></i>
            <input v-model="form.amount" type="number" id="amount" class="form-control" placeholder="Số tiền" min="1" required />
          </div>
        </div>
  
        <button type="submit" class="btn btn-primary">
          <i class="fas fa-paper-plane"></i> Gửi Yêu Cầu
        </button>
      </form>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { addWithdrawMoney } from '@/api/withdrawmoney.api';
  import type { AddWithDrawMoneyModel } from '@/models/addwithdrawmoney-model';
  import { userStore } from '@/stores/auth';
  
  export default defineComponent({
    name: 'WithdrawMoneyForm',
    setup() {
      const store = userStore();
      const userId = store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;
  
      const form = ref<AddWithDrawMoneyModel>({
        userId: userId,
        numberBank: '',
        nameBank: '',
        amount: 0
      });
  
      const errorMessage = ref<string | null>(null);
  
      // Danh sách ngân hàng với logo
      const banks = ref([
        { id: 1, name: 'Vietcombank', logo: '/images/banks/vietcombank.png' },
        { id: 2, name: 'BIDV', logo: '/images/banks/bidv.png' },
        { id: 3, name: 'Techcombank', logo: '/images/banks/techcombank.png' },
        { id: 4, name: 'VietinBank', logo: '/images/banks/vietinbank.png' },
        { id: 5, name: 'Sacombank', logo: '/images/banks/sacombank.png' }
      ]);
  
      const handleSubmit = async () => {
        try {
          const result = await addWithdrawMoney(form.value);
            alert('Yêu cầu rút tiền đã được gửi thành công!');
        } catch (error) {
          errorMessage.value = 'Không thể gửi yêu cầu rút tiền. Vui lòng thử lại.';
        }
      };
  
      return {
        form,
        errorMessage,
        handleSubmit,
        banks
      };
    }
  });
  </script>
  
  <style scoped>
  .withdraw-money-form {
    max-width: 600px;
    margin: auto;
    padding: 30px;
    background-color: #ffffff;
    border-radius: 10px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  }
  
  .title {
    text-align: center;
    font-size: 1.75rem;
    font-weight: 600;
    margin-bottom: 20px;
    color: #0073b1; /* LinkedIn blue */
  }
  
  .form-group {
    margin-bottom: 20px;
  }
  
  .input-group {
    display: flex;
    align-items: center;
    border-radius: 5px;
    border: 1px solid rgba(0, 0, 0, 0.1);
    padding: 10px;
    transition: border 0.3s ease;
  }
  
  .input-group i {
    font-size: 1.2rem;
    color: #6c757d;
    margin-right: 10px; /* Khoảng cách giữa icon và input */
  }
  
  .input-group input, .input-group select {
    border: none;
    outline: none;
    width: 100%;
    font-size: 1rem;
  }
  
  .input-group:hover {
    border-color: #0073b1; /* Màu viền khi hover */
  }
  
  select {
    width: 100%;
    padding: 10px;
    font-size: 1rem;
    border: 1px solid rgba(0, 0, 0, 0.1);
    border-radius: 5px;
    display: flex;
    align-items: center;
  }
  
  select:focus {
    border-color: #0073b1;
  }
  
  .bank-logo {
    width: 20px;
    height: 20px;
    margin-right: 10px;
  }
  
  
  button {
    width: 100%;
    padding: 12px;
    background-color: #0073b1;
    border: none;
    color: white;
    font-size: 1rem;
    font-weight: 600;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.3s ease;
  }
  
  button:hover {
    background-color: #005580;
  }
  
  button i {
    margin-right: 10px;
  }
  
  .alert {
    margin-bottom: 20px;
    background-color: #f8d7da;
    color: #721c24;
    padding: 10px;
    border-radius: 5px;
    border: 1px solid #f5c6cb;
  }
  </style>
  