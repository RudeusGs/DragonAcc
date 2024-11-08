// depositModels.ts

export interface AddDepositModel {
    UserId?: number;
    TelecomO?: string;
    DepositAmount?: number;
    NumberCard?: string;
    NumberSeri?: string;
    Status?: string;
  }
  
  export interface UpdateStatus_Model {
    Id?: number;
    Status?: string;
  }
  