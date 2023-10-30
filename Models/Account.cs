﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ShowroomManagement.Models
{
    public class Account
    {
        public Account()
        {
            Username = Password = string.Empty;    
        }

        public Account(string username, string password, string employeeId, int? level_account, bool? deleted, DateTime? createAt)
        {
            Username = username;
            Password = password;
            EmployeeId = employeeId;
            Level_account = level_account;
            Deleted = deleted;
            CreateAt = createAt;
            DeleteAt = null;
        }

        [Key]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Column(name: "Password_foruser", TypeName = "varbinary(500)")]
        [JsonPropertyName("password")]
        public string Password { get; set; }
        
        [JsonPropertyName("employeeId")]
        [Column("EmployeeId")]
        public string EmployeeId { get; set; }

        [JsonPropertyName("level_account")]
        public int? Level_account { get; set; } = 0;

        [JsonPropertyName("deleted")]
        public bool? Deleted { get; set; } = false;

        [JsonPropertyName("createAt")]
        public DateTime? CreateAt { get; set; }
        [JsonPropertyName("deleteAt")]
        public DateTime? DeleteAt { get; set; }

        //[JsonPropertyName("keepLoggined")]
        //public bool KeepLoggined { get; set; } = true;
    }
}
