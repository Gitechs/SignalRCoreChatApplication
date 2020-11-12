using System;
using System.Collections.Generic;

namespace ChatApplication.Utilities {
    public class FormatResponse<TData> {
        public FormatResponse () {

            this.Errors = new List<string> ();
        }
        public FormatResponse (bool isSuccessed, string message) : this () {
            this.IsSuccessed = isSuccessed;
            this.Message = message;
        }
        public void AddError (string errorMessage) => this.Errors.Add (errorMessage);

        public bool IsSuccessed { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }
        public List<String> Errors { get; set; }
    }
}