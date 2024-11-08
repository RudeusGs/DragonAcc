<template>
  <div class="container">
    <h2 class="title">Quản lý tài khoản</h2>
    <div class="account-tables">
      <!-- Table for League of Legends Accounts -->
      <div v-if="lolAccounts.length > 0" class="account-section">
        <h3 class="table-title">League of Legends</h3>
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Description</th>
              <th>Price</th>
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
              <td>{{ account.championCount }}</td>
              <td>{{ account.skinCount }}</td>
              <td>{{ account.rank }}</td>
              <td>{{ account.createdDate }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button class="btn-approve" @click="handleApproval('lol', account.id)">Duyệt</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div v-if="ngocRongAccounts.length > 0" class="account-section">
        <h3 class="table-title">Ngọc Rồng Online</h3>
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Description</th>
              <th>Price</th>
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
              <td>{{ account.server }}</td>
              <td>{{ account.planet }}</td>
              <td>{{ account.createdDate }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button class="btn-approve" @click="handleApproval('ngocRong', account.id)">Duyệt</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div v-if="valorantAccounts.length > 0" class="account-section">
        <h3 class="table-title">Valorant</h3>
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Description</th>
              <th>Price</th>
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
              <td>{{ account.gunSkinCount }}</td>
              <td>{{ account.championCount }}</td>
              <td>{{ account.rank }}</td>
              <td>{{ account.createdDate }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button class="btn-approve" @click="handleApproval('valorant', account.id)">Duyệt</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      
      <!-- Table for Tốc Chiến Accounts -->
      <div v-if="tocChienAccounts.length > 0" class="account-section">
        <h3 class="table-title">Tốc Chiến</h3>
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Description</th>
              <th>Price</th>
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
              <td>{{ account.championCount }}</td>
              <td>{{ account.skinCount }}</td>
              <td>{{ account.rank }}</td>
              <td>{{ account.createdDate }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button class="btn-approve" @click="handleApproval('tocChien', account.id)">Duyệt</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div v-if="errorMessage" class="error-message">
        {{ errorMessage }}
      </div>
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
    window.location.reload();
  };
  </script>
  
  <style scoped>
  .container {
    width: 100%;
    max-width: 1200px;
    margin: 20px auto;
    background-color: #fff;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    font-family: Arial, sans-serif;
  }
  
  .title {
    font-size: 24px;
    color: #0a66c2;
    text-align: center;
    margin-bottom: 20px;
  }
  
  .account-section {
    margin-top: 20px;
  }
  
  .table-title {
    margin-top: 10px;
    display: flex;
    justify-content: center;
    font-size: 18px;
    color: #0a66c2;
    margin-bottom: 10px;
  }
  
  .account-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 14px;
  }
  
  .account-table th {
    color: #333;
    font-weight: 600;
    padding: 10px;
    text-align: left;
    border-bottom: 2px solid #e5e7eb;
  }
  
  .account-table td {
    padding: 10px;
    border-bottom: 1px solid #e5e7eb;
    color: #555;
  }

  
  .btn-approve {
    padding: 6px 12px;
    background-color: #0a66c2;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    transition: background-color 0.3s ease;
  }
  
  .btn-approve:hover {
    background-color: #004182;
  }
  
  .error-message {
    color: #e74c3c;
    font-weight: bold;
    text-align: center;
    margin-top: 20px;
  }
  
  /* Responsive Design */
  @media (max-width: 768px) {
    .title {
      font-size: 20px;
    }
  
    .account-section {
      padding: 10px;
    }
  
    .account-table th,
    .account-table td {
      padding: 8px;
    }
  
    .btn-approve {
      padding: 5px 10px;
      font-size: 12px;
    }
  }
  
</style>

  