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
    public record AspNetUsersAspNetUserLogins_KEY_RESTResponseDto
    {
        public string LoginProvider { get; init; }
        public string ProviderKey { get; init; }
        public string? ProviderDisplayName { get; init; }
        public string UserId { get; init; }
    }
}