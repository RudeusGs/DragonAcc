<template>
    <div class="table-container">
      <table class="cute-table">
        <thead>
          <tr>
            <th>#</th>
            <th>Tên</th>
            <th>Email</th>
            <th>Vai trò</th>
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>1</td>
            <td>Nguyễn Văn A</td>
            <td>nguyenvana@example.com</td>
            <td>Quản trị viên</td>
            <td>
              <button class="action-btn edit"><i class="fas fa-edit"></i></button>
              <button class="action-btn delete"><i class="fas fa-trash-alt"></i></button>
            </td>
          </tr>
          <tr>
            <td>2</td>
            <td>Trần Thị B</td>
            <td>tranthib@example.com</td>
            <td>Người dùng</td>
            <td>
              <button class="action-btn edit"><i class="fas fa-edit"></i></button>
              <button class="action-btn delete"><i class="fas fa-trash-alt"></i></button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </template>
  
  <style scoped>
  /* Container */
  .table-container {
    display: flex;
    justify-content: center;
    padding: 20px;
    background-color: #f9f7fd;
  }
  
  /* Table Styling */
  .cute-table {
    width: 100%;
    max-width: 700px;
    border-collapse: collapse;
    font-family: Arial, sans-serif;
    background-color: #ffffff;
    border-radius: 12px;
    overflow: hidden;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  }
  
  /* Table Header */
  .cute-table thead tr {
    background-color: #ffe6f2; /* Màu pastel */
    color: #333;
    font-weight: bold;
  }
  
  .cute-table th, .cute-table td {
    padding: 12px 15px;
    text-align: center;
  }
  
  .cute-table th {
    font-size: 0.9rem;
  }
  
  /* Table Rows */
  .cute-table tbody tr {
    border-bottom: 1px solid #f0f0f0;
    transition: background-color 0.3s;
  }
  
  .cute-table tbody tr:hover {
    background-color: #fdf1ff; /* Màu pastel nhạt khi hover */
  }
  
  .cute-table td {
    font-size: 0.85rem;
    color: #555;
  }
  
  /* Action Buttons */
  .action-btn {
    border: none;
    background: transparent;
    cursor: pointer;
    font-size: 1rem;
    margin: 0 5px;
    transition: transform 0.2s;
  }
  
  .action-btn.edit {
    color: #4CAF50; /* Màu xanh lá cho nút sửa */
  }
  
  .action-btn.delete {
    color: #FF6B6B; /* Màu đỏ cho nút xóa */
  }
  
  .action-btn:hover {
    transform: scale(1.2);
  }
  
  /* Responsive */
  @media (max-width: 600px) {
    .cute-table th, .cute-table td {
      padding: 10px 8px;
    }
  }
  </style>
  
  