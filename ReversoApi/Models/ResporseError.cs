using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoApi.Models
{
    [Serializable]
    public class ResporseError
    {
        public bool Error { get; set; }
        public bool Success { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }
    }
}
