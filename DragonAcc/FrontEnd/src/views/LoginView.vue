<template>
  <LoadingSpinner :isLoading="loading" />
  <div class="login-container">
    <div class="login-card">
      <img class="image-Logo" src="../assets/LogoWebsite.png">
      <h3>Login</h3>
      <form @submit.prevent="handleLogin">
        <div class="input-group">
          <input  placeholder="Tên tài khoản" v-model="userName"  required />
        </div>
        <div class="input-group">
          <input type="password" placeholder="Mật khẩu" v-model="password" required />
        </div>
        <div class="forgot-password">
          <a href="#">Forgot Password?</a>
        </div>
        <button type="submit"  class="sign-in-btn">Sign in</button>
        <div class="or-container">
          <span>or continue with</span>
        </div>
        <div class="social-buttons">
          <button type="button" class="social-btn google-btn">
            <i class="fab fa-google"></i>
          </button>
          <button type="button" class="social-btn github-btn">
            <i class="fab fa-github"></i>
          </button>
          <button type="button" class="social-btn facebook-btn">
            <i class="fab fa-facebook-f"></i>
          </button>
        </div>
        <div class="register-link">
          <span>Don't have an account yet?</span> <a href="/register">Register for free</a>
        </div>
      </form>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import Cookies from "js-cookie";
import authApi from "@/api/authenticate.api";
import { userStore } from "../stores/auth";
export default defineComponent({
  setup() {
    const user = userStore();
    const userName = ref("");
    const password = ref("");   
    const router = useRouter();
    const loading = ref(true);
    const fetchData = async () => {
      loading.value = true;
      await new Promise((resolve) => setTimeout(resolve, 2000));
      loading.value = false;
    };
    const handleLogin = async () => {
      if (userName.value && password.value) {
        try {
          const loginModel = {
            userName: userName.value,
            password: password.value,
          };
          const response = await authApi.login(loginModel);
          if (response && response.result?.isSuccess) {
            user.login({
              id: response.result.data.userId,
              userName: response.result.data.username,
              email: response.result.data.email,
              fullName: response.result.data.fullName,
              balance: response.result.data.balance,
              coin: response.result.data.coin,
            });
            Cookies.set("token", response.result.data.token);
            localStorage.setItem('token', response.result.data.token);
            router.push("/");
            setTimeout(() => {
          window.location.reload();
        }, 100);
            
          } else {
            alert("Invalid username or password.");
          }
        } catch (error) {
          console.error("Error during login:", error);
          alert("API is not running.");
        }
      } else {
        alert("Username and password cannot be empty.");
      }
    };
    onMounted(async () => {
    fetchData();
    });
    return {
      userName,
      password,
      handleLogin,
      loading,
    };
  },
});
</script>

<style scoped>
.image-Logo {
  width: 50px;
  height: auto; 
  margin-bottom: 20px;
}
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
}

.login-card {
  background-color: rgba(255, 255, 255, 0.9);
  border-radius: 12px;
  padding: 30px 40px;
  box-shadow: 0 15px 30px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 400px;
  text-align: center;
}

h2 {
  margin-bottom: 20px;
  font-size: 24px;
  color: #0d47a1;
}

h3 {
  margin-bottom: 20px;
  font-size: 18px;
  color: #333;
}

.input-group {
  margin-bottom: 15px;
}

input {
  width: 100%;
  padding: 10px 15px;
  border: 1px solid #ddd;
  border-radius: 8px;
  font-size: 14px;
  outline: none;
}

.forgot-password {
  text-align: right;
  margin-bottom: 20px;
}

.forgot-password a {
  color: #0d47a1;
  font-size: 12px;
  text-decoration: none;
}

.sign-in-btn {
  width: 100%;
  padding: 10px;
  background-color: #0d47a1;
  color: #fff;
  border: none;
  border-radius: 8px;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.sign-in-btn:hover {
  background-color: #1976d2;
}

.or-container {
  margin: 20px 0;
  font-size: 12px;
  color: #666;
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
}

.or-container::before,
.or-container::after {
  content: '';
  flex: 1;
  height: 1px;
  background: #ddd;
  margin: 0 10px;
}

.social-buttons {
  display: flex;
  justify-content: center;
  gap: 10px;
  margin-bottom: 20px;
}

.social-btn {
  width: 40px;
  height: 40px;
  border: none;
  border-radius: 50%;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.google-btn {
  background-color: #db4437;
  color: #fff;
}

.github-btn {
  background-color: #333;
  color: #fff;
}

.facebook-btn {
  background-color: #3b5998;
  color: #fff;
}

.register-link {
  font-size: 12px;
  color: #666;
}

.register-link a {
  color: #0d47a1;
  text-decoration: none;
  font-weight: bold;
}
</style>
