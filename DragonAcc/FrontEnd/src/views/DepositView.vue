<template>
  <div class="overlay">
    <div class="form-container">
      <div class="form-header">
        <h2>Nạp tiền thẻ cào</h2>
      </div>
      <form @submit.prevent="submitTopUp">
        <div class="form-group">
          <label for="provider">Nhà cung cấp</label>
          <select v-model="provider" id="provider" class="form-control">
            <option value="Viettel">Viettel</option>
            <option value="Vinaphone">VinaPhone</option>
            <option value="Mobiphone">MobiPhone</option>
          </select>
        </div>

        <div class="form-group">
          <label>Chọn mệnh giá</label>
          <div class="amounts">
            <button
              v-for="(amount, index) in amounts"
              :key="index"
              :class="['amount-btn', { active: selectedAmount === amount }]"
              @click="selectAmount(amount)"
              type="button"
            >
              {{ amount }}<br />Nhận 80%
            </button>
          </div>
        </div>

        <div class="form-group">
          <label>Mã số thẻ</label>
          <input
            v-model="cardNumber"
            type="text"
            class="form-control"
            placeholder="Nhập mã số thẻ của bạn"
          />
        </div>

        <div class="form-group">
          <label>Số sê-ri</label>
          <input
            v-model="serialNumber"
            type="text"
            class="form-control"
            placeholder="Nhập mã số sê-ri trên thẻ"
          />
        </div>

        <div class="form-warning">
          <p>*Chú ý: Nạp thẻ sai mệnh giá mất 50% giá trị thẻ.</p>
        </div>

        <button type="submit" class="submit-btn" :disabled="loading">
          {{ loading ? 'Đang xử lý...' : 'Nạp ngay' }}
        </button>
        <p v-if="message" class="form-message">{{ message }}</p>
      </form>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue';
import apiClient from '@/api/base.api';
import { userStore } from '@/stores/auth';

export default {
  name: 'TopUpForm',
  setup() {
    const amounts = ref([10000, 20000, 30000, 50000, 100000, 200000, 300000, 500000, 1000000]);
    const selectedAmount = ref(10000); 
    const provider = ref('Viettel');
    const cardNumber = ref(''); 
    const serialNumber = ref(''); 
    const loading = ref(false); 
    const message = ref('');
    const store = userStore(); 
    const userId = store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;

    const selectAmount = (amount) => {
      selectedAmount.value = amount;
    };

    const submitTopUp = async () => {
  if (!cardNumber.value || !serialNumber.value) {
    message.value = 'Vui lòng nhập đủ thông tin!';
    return;
  }

  loading.value = true;
  message.value = '';
  const formData = new FormData();
  formData.append('UserId', userId.toString() || '');
  formData.append('TelecomO', provider.value || '');
  formData.append('DepositAmount', selectedAmount.value.toString() || '');
  formData.append('NumberCard', cardNumber.value || '');
  formData.append('NumberSeri', serialNumber.value || '');
  formData.append('Status', 'pending');

  try {
    const response = await apiClient.postForm('Deposit/add', formData);
    message.value = 'Gửi yêu cầu nạp thẻ thành công! Vui lòng đợi 3-5 phút';
  } catch (error) {
    console.error("API Error:", error);
    if (error.response) {
      message.value = `Lỗi: ${error.response.data.message || 'Đã xảy ra lỗi khi nạp thẻ. Vui lòng thử lại!'}`;
    } else {
      message.value = 'Đã xảy ra lỗi khi nạp thẻ. Vui lòng thử lại!';
    }
  } finally {
    loading.value = false;
  }
};
    return {
      amounts,
      selectedAmount,
      selectAmount,
      provider,
      cardNumber,
      serialNumber,
      loading,
      message,
      submitTopUp,
    };
  },
};
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body,
html {
  height: 100%;
  font-family: Arial, sans-serif;
  font-size: 12px;
}

.overlay {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: rgba(255, 255, 255, 0.5);
}

.form-container {
  background-color: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
  width: 100%;
  max-width: 600px;
}

.form-header {
  text-align: center;
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 10px;
}

.form-control {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 5px;
}

.amounts {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-top: 10px;
}

.amount-btn {
  flex: 1;
  padding: 15px;
  background-color: #f1f1f1;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  font-size: 12px;
  min-width: 120px;
  min-height: 80px;
  text-align: center;
}

.amount-btn.active {
  background-color: #0a66c2;
  color: white;
}

.form-warning {
  color: red;
  font-size: 10px;
  margin-bottom: 10px;
}

.submit-btn {
  width: 100%;
  padding: 8px;
  background-color: #0a66c2;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.form-message {
  text-align: center;
  color: green;
  font-size: 12px;
  margin-top: 10px;
}
</style>