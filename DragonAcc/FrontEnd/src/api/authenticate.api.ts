  import baseApi from './base.api';
  export default {
      login: async (model: LoginModel) : Promise<any|undefined> => {
        const response = await baseApi.postAuthenticate('Authenticate/login', model);
      if (response.status === 200) {
        return response.data;
      } else {
        throw new Error('Login failed');
      }
      },

      logout:()=>{
          baseApi.postAuthenticate("Authenticate/Logout",null);
      }, 
  };
  export interface LoginModel{
      userName: string,
      password: string,
  }
