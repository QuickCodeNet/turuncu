//------------------------------------------------------------------------------  
// <auto-generated>                                                               
// This code was generated by QuickCode.                                          
// Runtime Version:1.0                                                            
//                                                                                
// Changes to this file may cause incorrect behavior and will be lost if          
// the code is regenerated.                                                       
// </auto-generated>                                                              
//------------------------------------------------------------------------------  
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace QuickCode.Turuncu.UserManagerModule.Application.Dtos
{
    public record AspNetUsersRefreshTokens_KEY_RESTResponseDto
    {
        public int Id { get; init; }
        public string UserId { get; init; }
        public string Token { get; init; }
        public DateTime ExpiryDate { get; init; }
        public DateTime CreatedDate { get; init; }
        public bool IsRevoked { get; init; }
    }
}