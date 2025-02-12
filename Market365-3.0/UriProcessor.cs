﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market365_3._0 {
    /// <summary>
    /// Klasa UriPocesor umożliwia odwołanie się do konkretnych podstron z informacjami o produktach/zamówieniach
    /// </summary>
    public class UriProcessor {
        private string uri;
        /// <summary>
        /// Konstruktor przyjmuje adres w postaci kodu uri
        /// </summary>
        /// <param name="uri"></param>
        public UriProcessor(Uri uri) {
            this.uri = uri.ToString();
        }
        /// <summary>
        /// Konstruktor przyjmuje adres w postaci string
        /// </summary>
        /// <param name="uri"></param>
        public UriProcessor(string uri) {
            this.uri = uri.ToString();
        }

        /// <summary>
        /// args:
        /// [0] - idProduct
        /// [1] - buttonName
        /// [2](optional) - buttonNr
        /// </summary>
        /// <param name="args"></param>
        /// <returns>
        /// Zwraca adres strony uri jako string
        /// </returns>
        public string UriBuilder(string[] args) {
            if (args.Length == 2) {
                return uri + "/" +  args[1];
            }
            else if(args.Length == 2)
                return uri + "/" + args[1] + "/" + args[2];
            return uri;
        }


        /// <summary>
        /// Odczytuje parametry dołączone do linku URL
        /// </summary>
        /// <returns>
        /// Tablica parametrów otrzymanych w linku
        /// [0] - id strony/produktu
        /// [1..n](optional) - wartość parametru 1..n
        /// </returns>
        public string[] UriDecoder() {
            string[] buffer = uri.Split(new string[] { ".aspx"},0);
            if (buffer[1] != "") {
                buffer = buffer[1].Split('/');
                string[] output=new string[buffer.Length - 1];
                Array.Copy(buffer, 1, output,0, buffer.Length - 1);
                _ = output;

                return output;
            }
            else {
                return new string[] { "-1"};
            }
            
        }
    }
}