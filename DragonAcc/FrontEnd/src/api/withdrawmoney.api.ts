// src/api/withdrawMoney.api.ts
import baseapi from './base.api'; 
import type { AddWithDrawMoneyModel } from '../models/addwithdrawmoney-model';


export const addWithdrawMoney = async (model: AddWithDrawMoneyModel) => {
    const response = await baseapi.post('WithDrawMoney/add', model);
    return response.data;
};
export const getAll = async()=>{
    const reponse = await baseapi.get('WithDrawMoney/get-all');
    return reponse.data;
}
export const updateWithdrawStatus = async (id: number) => {
    const response = await baseapi.put(`WithDrawMoney/update-status?id=${id}`, {});
    return response.data;
};
