declare module 'vue-chartjs' {
    import { DefineComponent } from 'vue';
    import { ChartOptions, ChartData } from 'chart.js';
  
    export const Bar: DefineComponent<{
      chartData: ChartData;
      options?: ChartOptions;
    }>;
  
    export const Pie: DefineComponent<{
      chartData: ChartData;
      options?: ChartOptions;
    }>;
  
  }
  