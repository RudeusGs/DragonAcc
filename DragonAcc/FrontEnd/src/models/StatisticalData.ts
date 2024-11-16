// src/types/StatisticalData.ts

export interface StatisticalData {
    totalDeposit: number;      // Tổng số tiền nạp vào website
    countAccount: number;      // Tổng số tài khoản
    accountSold: number;       // Tổng số tài khoản bán được
    unSoldAccount: number;     // Tổng số tài khoản đang bán
    userId: number;            // ID của bảng thống kê
    totalWithDraw: number;     // Tổng số tiền đã rút
    id: number;                // ID của bản ghi thống kê
    createdDate: string;       // Ngày tạo
    updatedDate: string | null; // Ngày cập nhật
  }
  