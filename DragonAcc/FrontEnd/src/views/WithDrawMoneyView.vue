<template>
  <div class="withdraw-money-form">
    <h4 class="title">Yêu Cầu Rút Tiền</h4>

    <div v-if="errorMessage" class="alert alert-danger">{{ errorMessage }}</div>

    <form @submit.prevent="handleSubmit">
      <!-- Tên Ngân Hàng -->
      <div class="form-group">
        <label for="nameBank">Tên Ngân Hàng</label>
        <div class="input-group">
          <i class="fas fa-building icon"></i>
          <select v-model="form.nameBank" id="nameBank" class="form-control" required>
            <option value="" disabled selected>Chọn ngân hàng</option>
            <option v-for="bank in banks" :key="bank.id" :value="bank.name">
              {{ bank.name }}
            </option>
          </select>
        </div>
      </div>

      <!-- Số Tài Khoản Ngân Hàng -->
      <div class="form-group">
        <label for="numberBank">Số Tài Khoản Ngân Hàng</label>
        <div class="input-group">
          <i class="fas fa-credit-card icon"></i>
          <input v-model="form.numberBank" type="text" id="numberBank" class="form-control" placeholder="Số tài khoản ngân hàng" required />
        </div>
      </div>

      <!-- Số Tiền Rút -->
      <div class="form-group">
        <label for="amount">Số Tiền Rút</label>
        <div class="input-group">
          <i class="fas fa-dollar-sign icon"></i>
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

    const banks = ref([
      { id: 1, name: 'Vietcombank', logo: '/images/banks/vietcombank.png' },
      { id: 2, name: 'BIDV', logo: '/images/banks/bidv.png' },
      { id: 3, name: 'Techcombank', logo: '/images/banks/techcombank.png' },
      { id: 4, name: 'VietinBank', logo: '/images/banks/vietinbank.png' },
      { id: 5, name: 'Sacombank', logo: '/images/banks/sacombank.png' },
      { id: 6, name: 'Shinhanbank', logo: '/images/banks/shinhanbank.png' },
      { id: 7, name: 'Mbbank', logo: '/images/banks/mbbank.png' },
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
  max-width: 500px;
  margin: 50px auto;
  padding: 40px;
  background-color: #f8f9fa;
  border-radius: 12px;
  box-shadow: 0 8px 40px rgba(0, 0, 0, 0.15);
}

.title {
  text-align: center;
  font-size: 1.75rem;
  font-weight: 600;
  color: #0073b1;
  margin-bottom: 30px;
  text-transform: uppercase;
  letter-spacing: 1px;
}

.form-group {
  margin-bottom: 25px;
}

.form-group label {
  font-size: 1rem;
  font-weight: 500;
  color: #495057;
  margin-bottom: 8px;
  display: block;
}

.input-group {
  display: flex;
  align-items: center;
  background-color: #ffffff;
  border: 1px solid #ced4da;
  border-radius: 8px;
  padding: 10px 15px;
  transition: all 0.3s ease;
}

.input-group:hover {
  border-color: #0073b1;
  box-shadow: 0 0 8px rgba(0, 115, 177, 0.2);
}

.input-group .icon {
  font-size: 1.2rem;
  color: #0073b1;
  margin-right: 12px;
}

.form-control {
  flex: 1;
  border: none;
  outline: none;
  font-size: 1rem;
  color: #495057;
}

.form-control::placeholder {
  color: #adb5bd;
}

.btn-primary {
  width: 100%;
  padding: 12px;
  background-color: #0073b1;
  border: none;
  color: #ffffff;
  font-size: 1.1rem;
  font-weight: bold;
  border-radius: 8px;
  cursor: pointer;
  transition: background-color 0.3s ease, transform 0.1s ease;
}

.btn-primary:hover {
  background-color: #005580;
}

.btn-primary:active {
  transform: scale(0.98);
}

.alert {
  padding: 15px;
  color: #721c24;
  background-color: #f8d7da;
  border: 1px solid #f5c6cb;
  border-radius: 8px;
  margin-bottom: 20px;
  text-align: center;
  font-size: 0.9rem;
}
</style>
