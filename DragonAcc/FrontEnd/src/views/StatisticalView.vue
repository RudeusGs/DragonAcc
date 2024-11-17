<template>
  <div class="container">
    <div v-if="!data">
      <div class="alert alert-warning">
        Không có dữ liệu thống kê cho người dùng này.
      </div>
    </div>

    <div v-else>
      <div class="row my-3">
        <div class="col">
          <h4>Dữ liệu Thống Kê Doanh Thu Và Tài Khoản</h4>
        </div>
      </div>

      <div class="row my-2">
        <div class="col-md-6 py-1">
          <div class="card">
            <div class="card-body">
              <canvas ref="chLine"></canvas>
            </div>
          </div>
        </div>
        <div class="col-md-6 py-1">
          <div class="card">
            <div class="card-body">
              <canvas ref="chBar"></canvas>
            </div>
          </div>
        </div>
      </div>

      <div class="row py-2">
        <div class="col-md-4 py-1" v-for="(donut, index) in donutCharts" :key="index">
          <div class="card">
            <div class="card-body">
              <canvas ref="chDonutRefs"></canvas>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, nextTick } from 'vue';
import { getStatisticsByUserId } from '@/api/statistical.api';
import type { StatisticalData } from '@/models/StatisticalData';
import { userStore } from '@/stores/auth';
import Chart from 'chart.js/auto';

export default defineComponent({
  name: 'StatisticalView',
  setup() {
    const data = ref<StatisticalData | null>(null);
    const donutCharts = [1, 2, 3]; 

    const chLine = ref<HTMLCanvasElement | null>(null);
    const chBar = ref<HTMLCanvasElement | null>(null);
    const chDonutRefs = ref<HTMLCanvasElement[]>([]); 

    onMounted(async () => {
      const store = userStore();
      const userData = JSON.parse(localStorage.getItem('user') || '{}');
      const userId = store.user?.id || userData.id;

      try {
        const response = await getStatisticsByUserId.getByUserId(userId);
        if (response.result.isSuccess && response.result.data.length > 0) {
          data.value = response.result.data[0];
          await nextTick();
          initializeCharts();
        } else {
          console.warn('Không có dữ liệu từ API cho userId:', userId);
        }
      } catch (error) {
        console.error('Error fetching statistics:', error);
      }
    });

    const formatCurrency = (amount: number): string => {
      return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
    };

    const initializeCharts = () => {
      if (!data.value) return;
      if (chLine.value) {
        new Chart(chLine.value, {
          type: 'line',
          data: {
            labels: ['Tổng Chi Tiêu', 'Tổng Nạp', 'Tổng Rút', 'Tổng Kiếm Được'],
            datasets: [{
              label: 'Số Tiền (VND)',
              data: [
                data.value.totalEarnings,
                data.value.totalDeposit,
                data.value.totalWithdrawn,
                data.value.totalEarnings
              ],
              backgroundColor: 'rgba(0, 123, 255, 0.2)',
              borderColor: '#007bff',
              borderWidth: 2,
              fill: true,
              tension: 0.4
            }]
          },
          options: {
            responsive: true,
            plugins: {
              legend: { display: false },
              tooltip: {
                callbacks: {
                  label: function(context) {
                    return formatCurrency(context.parsed.y);
                  }
                }
              }
            },
            scales: {
              y: {
                beginAtZero: true,
                ticks: {
                  callback: function(value) {
                    return formatCurrency(Number(value));
                  }
                }
              }
            }
          }
        });
      }
      if (chBar.value) {
        new Chart(chBar.value, {
          type: 'bar',
          data: {
            labels: ['Số Tài Khoản Hiện Có', 'Tài Khoản Đã Bán', 'Tài Khoản Chưa Bán', 'Bán Thành Công', 'Mua Thành Công'],
            datasets: [{
              label: 'Số Lượng',
              data: [
                data.value.currentAccountCount,
                data.value.soldAccountCount,
                data.value.unsoldAccountCount,
                data.value.successfulSalesCount,
                data.value.successfulPurchasesCount
              ],
              backgroundColor: ['#007bff', '#28a745', '#dc3545', '#ffc107', '#17a2b8'],
              borderWidth: 1
            }]
          },
          options: {
            responsive: true,
            plugins: {
              legend: { display: false },
              tooltip: {
                callbacks: {
                  label: function(context) {
                    return `${context.label}: ${context.parsed.y}`;
                  }
                }
              }
            },
            scales: {
              y: {
                beginAtZero: true
              }
            }
          }
        });
      }

      const donutDataExamples = [
        {
          labels: ['Tài Khoản Bán Được', 'Tài Khoản Chưa Bán'],
          data: [data.value.soldAccountCount, data.value.unsoldAccountCount],
          backgroundColor: ['#28a745', '#dc3545']
        },
        {
          labels: ['Tổng Tiền Nạp', 'Tổng Tiền Rút'],
          data: [data.value.totalDeposit, data.value.totalWithdrawn],
          backgroundColor: ['#007bff', '#ffc107']
        },
        {
          labels: ['Thành Công Bán', 'Thất Bại Bán'],
          data: [data.value.successfulSalesCount, data.value.currentAccountCount - data.value.successfulSalesCount],
          backgroundColor: ['#17a2b8', '#6c757d']
        }
      ];

      chDonutRefs.value.forEach((canvas, index) => {
        if (canvas) {
          const donutData = donutDataExamples[index];
          new Chart(canvas, {
            type: 'doughnut',
            data: {
              labels: donutData.labels,
              datasets: [{
                data: donutData.data,
                backgroundColor: donutData.backgroundColor,
                borderWidth: 0
              }]
            },
            options: {
              responsive: true,
              plugins: {
                legend: {
                  position: 'bottom',
                  labels: {
                    usePointStyle: true,
                    pointStyle: 'circle'
                  }
                },
                tooltip: {
                  callbacks: {
                    label: function(context) {
                      const label = context.label || '';
                      const value = context.parsed;
                      return `${label}: ${value}`;
                    }
                  }
                }
              }
            }
          });
        }
      });
    };

    return {
      data,
      formatCurrency,
      donutCharts,
      chLine,
      chBar,
      chDonutRefs
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
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}
canvas {
  width: 100% !important;
  height: auto !important;
}
</style>
