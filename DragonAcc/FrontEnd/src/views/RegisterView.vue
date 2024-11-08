<template>
    <div class="registration-container mt-5">
      <div class="registration-header">
        <h2>Join DA</h2>
        <p>Social network for gamers</p>
      </div>
  
      <form @submit.prevent="registerUser">
        <div class="form-group">
          <label for="accountName">Account Name</label>
          <input
            type="text"
            id="accountName"
            v-model="accountName"
            placeholder="Enter your account name"
            required
          />
        </div>
  
        <div class="form-group">
          <label for="email">Email</label>
          <input
            type="email"
            id="email"
            v-model="email"
            placeholder="Enter your email"
            required
          />
        </div>
  
        <div class="form-group">
          <label for="fullName">Fullname</label>
          <input
            type="text"
            id="fullName"
            v-model="fullName"
            placeholder="Enter your full name"
            required
          />
        </div>
  
        <div class="form-group">
          <label for="password">Password</label>
          <input
            type="password"
            id="password"
            v-model="password"
            placeholder="Enter your password"
            required
          />
        </div>
  
        <div class="form-group">
          <label for="confirmPassword">Confirm Password</label>
          <input
            type="password"
            id="confirmPassword"
            v-model="confirmPassword"
            placeholder="Re-enter your password"
            required
          />
        </div>
  
        <button type="submit" class="submit-btn">Agree & Join</button>
      </form>
  
      <p class="terms">
        By clicking Agree & Join, you agree to DA's
        <a href="#">User Agreement</a>, <a href="#">Privacy Policy</a>, and
        <a href="#">Cookie Policy</a>.
      </p>
  
      <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
      <p v-if="successMessage" class="success-message">{{ successMessage }}</p>
    </div>
  </template>
  
  <script>
  import api from '@/api/base.api';
  
  export default {
    name: "RegistrationForm",
    data() {
      return {
        accountName: "",
        email: "",
        fullName: "",
        password: "",
        confirmPassword: "",
        errorMessage: "",
        successMessage: "",
      };
    },
    methods: {
  async registerUser() {
    this.errorMessage = "";
    this.successMessage = "";

    if (this.password !== this.confirmPassword) {
      this.errorMessage = "Passwords do not match";
      return;
    }
    try {
      const response = await api.register(
        this.accountName,
        this.password,
        this.fullName,
        this.email
      );
      this.successMessage = "Registration successful!";
      this.clearForm();
    } catch (error) {
      if (error.response) {
        this.errorMessage = error.response.data.message || "Registration failed. Please try again.";
      } else {
        this.errorMessage = "An unexpected error occurred. Please try again.";
      }
    }
  },

  clearForm() {
    this.accountName = "";
    this.email = "";
    this.fullName = "";
    this.password = "";
    this.confirmPassword = "";
  }
}
  };
  </script>
  
  <style scoped>
  .registration-container {
    width: 100%;
    max-width: 400px;
    background-color: white;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    margin: auto;
    text-align: center;
  }
  
  .registration-header h2 {
    color: #0a66c2;
    font-size: 24px;
    margin-bottom: 8px;
  }
  
  .registration-header p {
    color: #666;
    font-size: 14px;
    margin-bottom: 20px;
  }
  
  .form-group {
    margin-bottom: 15px;
    text-align: left;
  }
  
  .form-group label {
    display: block;
    color: #333;
    font-size: 14px;
    margin-bottom: 5px;
  }
  
  .form-group input {
    width: 100%;
    padding: 10px;
    font-size: 14px;
    border: 1px solid #ccc;
    border-radius: 4px;
    transition: border-color 0.3s;
  }
  
  .form-group input:focus {
    border-color: #0a66c2;
    outline: none;
  }
  
  .submit-btn {
    width: 100%;
    padding: 10px;
    background-color: #0a66c2;
    color: white;
    border: none;
    border-radius: 4px;
    font-size: 16px;
    cursor: pointer;
    transition: background-color 0.3s;
  }
  
  .submit-btn:hover {
    background-color: #004182;
  }
  
  .terms {
    font-size: 12px;
    color: #666;
    margin-top: 10px;
  }
  
  .terms a {
    color: #0a66c2;
    text-decoration: none;
  }
  
  .terms a:hover {
    text-decoration: underline;
  }
  
  .error-message {
    color: red;
    font-size: 14px;
    margin-top: 10px;
  }
  
  .success-message {
    color: green;
    font-size: 14px;
    margin-top: 10px;
  }
  </style>
  