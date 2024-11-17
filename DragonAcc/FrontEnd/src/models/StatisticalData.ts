export interface StatisticalData {
  totalDeposit: number;
  totalWithdrawn: number;      
  totalEarnings: number;      
  totalAccountSales: number;
  totalAccountPurchases: number;
  currentAccountCount: number;
  soldAccountCount: number;   
  unsoldAccountCount: number; 
  successfulSalesCount: number;
  successfulPurchasesCount: number;
  draftAccountsCount: number;
  userId: number;
  createdDate: string;
  updatedDate: string | null;
}
