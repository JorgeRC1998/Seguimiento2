using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Gps.Model;
using RestSharp;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Http;

namespace Gps
{
    class GestorGPS
    {
        private static readonly HttpClient client = new HttpClient();
        public double consultarDistancia(decimal a1, decimal b1)
        {
            double lon = ConsultarCoordenadas().ModelLon;
            double lat = ConsultarCoordenadas().ModelLat;
            
            double aux1 = 7.0000;
            double aux2 = 5.0000;
            var y2 = b1;
            var x2 = a1;
            double result = 0.0;

            if ((a1.ToString() == "") || (b1.ToString() == ""))
            {
                Console.WriteLine("Error llena los dos campos");
            }
            else
            {
                result = Math.Sqrt(Math.Pow((Convert.ToDouble(x2) - lon), 2) + (Math.Pow((Convert.ToDouble(y2) - lat), 2)));
            }

            return result;

        }

        public Punto ConsultarCoordenadas()
        {
            Punto objPunto = new Punto();
            int x = 0;
            var client = new RestClient($"http://0cb1545d.ngrok.io/API/index.php/select");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                

                Console.WriteLine(response.Content);
                JObject jsonCoordenadas = JObject.Parse("{datos:" + response.Content.ToString() + "}");
                Console.WriteLine(response.Content.ToString());

                foreach(object item in jsonCoordenadas["datos"]["datos"])
                {
                    x = x + 1;
                }
                #pragma warning disable CS1717 // Se ha asignado a la misma variable
                x = x;
                #pragma warning restore CS1717 // Se ha asignado a la misma variable
                objPunto.ModelId = Convert.ToInt32(jsonCoordenadas ["datos"]["datos"][x-1]["ID"]);
                objPunto.ModelLon = Convert.ToDouble(jsonCoordenadas["datos"]["datos"][x-1]["LONGITUD"]);
                objPunto.ModelLat = Convert.ToDouble(jsonCoordenadas["datos"]["datos"][x-1]["LATITUD"]);
                objPunto.ModelFech = Convert.ToDateTime(jsonCoordenadas["datos"]["datos"][x-1]["FECHA"]);
                Console.WriteLine("Numero de elementos:" + (x));
            }
            else
            {
                Console.WriteLine("Error");
            }

            return objPunto;
        }

        public async void mandarCoordenadas(double x1, double y1)
        {
            double latitud = x1;
            double longitud = y1;
            DateTime fecha = DateTime.Now;
            string año = fecha.Year.ToString();
            string mes = fecha.Month.ToString();
            string dia = fecha.Day.ToString();
            decimal hora = (fecha.Hour - 1);
            string minuto = fecha.Minute.ToString();
            string segundo = fecha.Second.ToString();
            string consFecha = ($"{año}-{mes}-{dia}  {hora}:{minuto}:{segundo}");

            //var client = new RestClient("http://0cb1545d.ngrok.io/API/index.php/insertar");
            //var request = new RestRequest(Method.POST);
            //request.AddParameter("undefined", "{\r\n\"LONGITUD\": \"{longitud}\",\r\n\"latitud\": \"1.0\",\r\n\"FECHA\": \"2018-07-20 09:55:00\"\r\n}", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(request);

            var values = new Dictionary<string, string>
            {
                {"LONGITUD", $"{longitud}" },
                {"LATITUD", $"{latitud}" },
                {"FECHA", $"{consFecha}" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://0cb1545d.ngrok.io/API/index.php/insertar", content);

            var responseString = await response.Content.ReadAsStringAsync();
        }

    }
 }
 


