using System;

namespace DataTransferObject.Dto
{
    public class Result<T>
    {

        T _resultData;

        public Result()
        {
        }

        public Result(T resultData)
        {
            _resultData = resultData;
            this.Data = resultData;
        }

        /// <summary>
        /// Gelen veri boş ise true mesajını döndürür
        /// </summary>
        public bool IsNull
        {
            get
            {
                bool value = false;
                if (_resultData == null)
                {
                    value = true;
                }
                return value;
            }
        }

        /// <summary>
        /// Gelen veri hatalı ise hata mesajını, değil ise başarılı mesajını döndürür
        /// </summary>
        public string Message
        {
            get
            {
                string message = string.Empty;
                try
                {
                    if (_resultData.ToString().Equals(string.Empty))
                    {
                        message = "İşlem Başarılı";
                    }
                }
                catch (Exception e)
                {
                    if (e.HResult == new NullReferenceException().HResult)
                    {
                        message = "Kayıt Bulunamadı veya hatalı istek! :(";
                    }
                    else if (e.Data.Count <= 0)
                    {
                        message = "Aradığınız kayıt bulunamadı! :(";
                    }
                    else
                    {
                        message = "Üzgünüz fakat beklenmeyen bir hata oluştu :(";
                    }
                }
                return message;
            }
            set
            {
                Message = value;
            }
        }
        public T Data { get; set; }
    }
}
