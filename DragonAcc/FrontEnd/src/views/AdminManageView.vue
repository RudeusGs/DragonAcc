<template>
    <div class="container">
      <h2 class="d-flex justify-content-center">Quản trị</h2>
      <div class="account-tables">
        <!-- Table for League of Legends Accounts -->
        <div v-if="lolAccounts.length > 0">
          <h3>Liên minh huyền thoại</h3>
          <table class="account-table">
            <thead>
              <tr>
                <th>Tài khoản</th>
                <th>Password</th>
                <th>Password Changed</th>
                <th>Description</th>
                <th>Price</th>               
                <th>Nhận tiền</th>
                <th>Champion Count</th>
                <th>Skin Count</th>
                <th>Rank</th>
                <th>Created Date</th>
                <th>Status</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(account, index) in lolAccounts" :key="index">
                <td>{{ account.accountName }}</td>
                <td>{{ account.password }}</td>
                <td>{{ account.passwordChanged }}</td>
                <td>{{ account.description }}</td>
                <td>{{ account.price }}</td>
                <td>{{ account.noYetMoney }}</td>
                <td>{{ account.championCount }}</td>
                <td>{{ account.skinCount }}</td>
                <td>{{ account.rank }}</td>
                <td>{{ account.createdDate }}</td>
                <td>{{ account.status }}</td>
                <td>
                  <button class="btn-duyet" @click="handleApproval('lol', account.id)">Duyệt</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
  
        <!-- Table for Ngọc Rồng Accounts -->
        <div v-if="ngocRongAccounts.length > 0">
          <h3>Ngọc Rồng Online</h3>
          <table class="account-table">
            <thead>
              <tr>
                <th>Tài khoản</th>
                <th>Password</th>
                <th>Password Changed</th>
                <th>Description</th>
                <th>Price</th>               
                <th>Nhận tiền</th>
                <th>Server</th>
                <th>Planet</th>
                <th>Created Date</th>
                <th>Status</th>
                <th>Action</th>  
              </tr>              
            </thead>
            <tbody>
              <tr v-for="(account, index) in ngocRongAccounts" :key="index">
                <td>{{ account.accountName }}</td>
                <td>{{ account.password }}</td>
                <td>{{ account.passwordChanged }}</td>
                <td>{{ account.description }}</td>
                <td>{{ account.price }}</td>
                <td>{{ account.noYetMoney }}</td>
                <td>{{ account.server }}</td>
                <td>{{ account.planet }}</td>
                <td>{{ account.createdDate }}</td>
                <td>{{ account.status }}</td>
                <td>
                  <button class="btn-duyet" @click="handleApproval('ngocRong', account.id)">Duyệt</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
  
        <!-- Table for PUBG Accounts -->
        <div v-if="pubgAccounts.length > 0">
          <h3>Pubg</h3>
          <table class="account-table">
            <thead>
              <tr>
                <th>Tài khoản</th>
                <th>Password</th>
                <th>Password Changed</th>
                <th>Description</th>
                <th>Price</th>             
                <th>Nhận tiền</th>
                <th>Gun Skin Count</th>
                <th>Human Skin Count</th>
                <th>Rank</th>
                <th>Created Date</th>
                <th>Status</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(account, index) in pubgAccounts" :key="index">
                <td>{{ account.accountName }}</td>
                <td>{{ account.password }}</td>
                <td>{{ account.passwordChanged }}</td>
                <td>{{ account.description }}</td>
                <td>{{ account.price }}</td>              
                <td>{{ account.noYetMoney }}</td>
                <td>{{ account.gunSkinCount }}</td>
                <td>{{ account.humanSkinCount }}</td>
                <td>{{ account.rank }}</td>
                <td>{{ account.createdDate }}</td>
                <td>{{ account.status }}</td>
                <td>
                  <button class="btn-duyet" @click="handleApproval('pubg', account.id)">Duyệt</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
  
        <!-- Table for Tốc Chiến Accounts -->
        <div v-if="tocChienAccounts.length > 0">
          <h3>Tốc Chiến</h3>
          <table class="account-table">
            <thead>
              <tr>
                <th>Tài khoản</th>
                <th>Password</th>
                <th>Password Changed</th>
                <th>Description</th>
                <th>Price</th>                
                <th>Nhận tiền</th>
                <th>Champion Count</th>
                <th>Skin Count</th>
                <th>Rank</th>
                <th>Created Date</th>
                <th>Status</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(account, index) in tocChienAccounts" :key="index">
                <td>{{ account.accountName }}</td>
                <td>{{ account.password }}</td>
                <td>{{ account.passwordChanged }}</td>
                <td>{{ account.description }}</td>
                <td>{{ account.price }}</td>
                <td>{{ account.noYetMoney }}</td>
                <td>{{ account.championCount }}</td>
                <td>{{ account.skinCount }}</td>
                <td>{{ account.rank }}</td>
                <td>{{ account.createdDate }}</td>
                <td>{{ account.status }}</td>
                <td>
                  <button class="btn-duyet" @click="handleApproval('tocChien', account.id)">Duyệt</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
  
        <!-- Table for Valorant Accounts -->
        <div v-if="valorantAccounts.length > 0">
          <h3>Valorant</h3>
          <table class="account-table">
            <thead>
              <tr>
                <th>Tài khoản</th>
                <th>Password</th>
                <th>Password Changed</th>
                <th>Description</th>
                <th>Price</th>                
                <th>Nhận tiền</th>
                <th>Gun Skin Count</th>
                <th>Champion Count</th>
                <th>Rank</th>
                <th>Created Date</th>
                <th>Status</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(account, index) in valorantAccounts" :key="index">
                <td>{{ account.accountName }}</td>
                <td>{{ account.password }}</td>
                <td>{{ account.passwordChanged }}</td>
                <td>{{ account.description }}</td>
                <td>{{ account.price }}</td>
                <td>{{ account.noYetMoney }}</td>
                <td>{{ account.gunSkinCount }}</td>
                <td>{{ account.championCount }}</td>
                <td>{{ account.rank }}</td>
                <td>{{ account.createdDate }}</td>
                <td>{{ account.status }}</td>
                <td>
                  <button class="btn-duyet" @click="handleApproval('valorant', account.id)">Duyệt</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <div v-if="errorMessage" class="error-message">
        {{ errorMessage }}
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted } from 'vue';
  import getallgameaccountAPI from '@/api/getallgameaccount.api';
  import ManageUpdateForAdminAPI from '@/api/manageforadmin.api';
  import type { Lol_Game, NgocRong_Game, Pubg_Game, TocChien_Game, Valorant_Game } from '@/models/addaccount-model';
  
  const lolAccounts = ref<Lol_Game[]>([]);
  const ngocRongAccounts = ref<NgocRong_Game[]>([]);
  const pubgAccounts = ref<Pubg_Game[]>([]);
  const tocChienAccounts = ref<TocChien_Game[]>([]);
  const valorantAccounts = ref<Valorant_Game[]>([]);
  
  const errorMessage = ref<string | null>(null);
  
  onMounted(async () => {
    try {
      const [lolRes, ngocRongRes, pubgRes, tocChienRes, valorantRes] = await Promise.all([
        getallgameaccountAPI.getAllLOL(),
        getallgameaccountAPI.getAllNgocRong(),
        getallgameaccountAPI.getAllPubg(),
        getallgameaccountAPI.getAllTocChien(),
        getallgameaccountAPI.getAllValorant()
      ]);
  
      lolAccounts.value = lolRes.data.result?.data || [];
      ngocRongAccounts.value = ngocRongRes.data.result?.data || [];
      pubgAccounts.value = pubgRes.data.result?.data || [];
      tocChienAccounts.value = tocChienRes.data.result?.data || [];
      valorantAccounts.value = valorantRes.data.result?.data || [];
    } catch (error) {
      console.error("Error fetching game accounts:", error);
    }
  });
  
  const handleApproval = (gameType: string, accountId: number | null | undefined) => {
    if (!accountId) {
      errorMessage.value = "Invalid account ID. Please check and try again.";
      return;
    }
  
    approveAccount(gameType, accountId);
  };
  
  const approveAccount = async (gameType: string, accountId: number) => {
    try {
      let response;
      switch (gameType) {
        case 'lol':
          response = await ManageUpdateForAdminAPI.ManageUpdateLOL(accountId);
          break;
        case 'ngocRong':
          response = await ManageUpdateForAdminAPI.ManageUpdateNgocRong(accountId);
          break;
        case 'pubg':
          response = await ManageUpdateForAdminAPI.ManageUpdatePubg(accountId);
          break;
        case 'tocChien':
          response = await ManageUpdateForAdminAPI.ManageUpdateTocChien(accountId);
          break;
        case 'valorant':
          response = await ManageUpdateForAdminAPI.ManageUpdateValorant(accountId);
          break;
        default:
          throw new Error("Invalid game type");
      }
      errorMessage.value = null;
      alert("Account approved successfully!");
      console.log(response);
    } catch (error) {
      console.error("Error approving account:", error);
      errorMessage.value = "Failed to approve the account. Please try again.";
    }
  };
  </script>
  
  <style scoped>
  /* Container and Title Styling */
  .container {
    padding: 30px;
    max-width: 1600px;
    margin: 0 auto;
    font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
    color: #3b3b3b;
    background-color: #f8f9fa;
    border-radius: 10px;
    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
    animation: fadeIn 1s ease-in-out;
  }
  
  .title {
    font-size: 26px;
    font-weight: 800;
    color: #0073b1;
    text-align: center;
    margin-bottom: 25px;
    animation: slideDown 0.6s ease-out;
  }
  
  /* Account Section Styling */
  .account-tables {
    display: flex;
    flex-direction: column;
    gap: 20px;
  }
  
  .account-section {
    background-color: #ffffff;
    border-radius: 15px;
    padding: 20px;
    box-shadow: 0 8px 20px rgba(0, 0, 0, 0.1);
    transition: transform 0.3s, box-shadow 0.3s;
    opacity: 0;
    transform: translateY(20px);
    animation: slideUp 0.8s ease-in forwards;
  }
  
  .account-section:hover {
    transform: translateY(-5px);
    box-shadow: 0 12px 25px rgba(0, 0, 0, 0.15);
  }
  
  /* Account Table Styling */
  .account-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 14px;
    background-color: #fff;
  }
  
  .account-table th {
    background-color: #0073b1;
    color: #ffffff;
    font-weight: 600;
    padding: 12px;
    text-align: left;
    border-bottom: 2px solid #004161;
  }
  
  .account-table td {
    padding: 12px;
    border-bottom: 1px solid #e1e9ee;
    color: #3b3b3b;
    transition: background-color 0.3s;
  }
  
  .account-table tr:hover td {
    background-color: #f1f3f5;
    transform: scale(1.01);
  }
  
  /* Button Styling with Ripple Effect */
  .btn-duyet {
    position: relative;
    overflow: hidden;
    height: 35px;
    width: 100px;
    color: white;
    border: none;
    border-radius: 25px;
    font-size: 14px;
    font-weight: bold;
    cursor: pointer;
    background-color: #0073b1;
    transition: background 0.3s, transform 0.2s;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
    text-align: center;
  }
  
  .btn-duyet:hover {
    transform: translateY(-3px);
  }
  
  .btn-duyet:active {
    background-color: #004161;
    transform: translateY(0);
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2) inset;
  }
  
  .ripple-effect::after {
    content: '';
    position: absolute;
    top: 50%;
    left: 50%;
    width: 0;
    height: 0;
    background: rgba(255, 255, 255, 0.5);
    border-radius: 50%;
    transform: translate(-50%, -50%);
    animation: ripple 0.6s linear;
  }
  
  @keyframes ripple {
    from {
      width: 0;
      height: 0;
      opacity: 1;
    }
    to {
      width: 200px;
      height: 200px;
      opacity: 0;
    }
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
      opacity: 0;
    }
    to {
      transform: translateY(0);
      opacity: 1;
    }
  }
  
  @keyframes slideUp {
    from {
      opacity: 0;
      transform: translateY(20px);
    }
    to {
      opacity: 1;
      transform: translateY(0);
    }
  }
  
  /* Error Message Styling */
  .error-message {
    color: #e74c3c;
    font-weight: bold;
    text-align: center;
    margin-top: 20px;
  }
  
  /* Responsive Design */
  @media (max-width: 768px) {
    .account-section {
      padding: 15px;
    }
  
    .account-table th, .account-table td {
      padding: 10px;
    }
  
    .btn-duyet {
      width: 80px;
      font-size: 12px;
    }
  }
  </style>
  