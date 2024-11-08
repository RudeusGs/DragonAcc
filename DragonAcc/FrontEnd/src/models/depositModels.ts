// depositModels.ts

// Model for adding a new deposit
export interface AddDepositModel {
    UserId?: number;
    TelecomO?: string;
    DepositAmount?: number;
    NumberCard?: string;
    NumberSeri?: string;
    Status?: string;
  }
  
  // Model for updating the status of a deposit
  export interface UpdateStatus_Model {
    Id?: number;
    Status?: string;
  }
  