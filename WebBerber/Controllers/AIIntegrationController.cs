﻿using Microsoft.AspNetCore.Mvc;
using RestSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;

namespace WebBerber.Controllers
{
    public class AIIntegrationController : Controller
    {

        private byte[] ResizeImage(byte[] imageBytes, int width, int height)
        {
            using var inputStream = new MemoryStream(imageBytes);
            using var outputStream = new MemoryStream();
            using var image = Image.Load(inputStream);

            // Resim boyutunu ayarlama
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(width, height)
            }));

            image.SaveAsJpeg(outputStream);
            return outputStream.ToArray();
        }


        [HttpPost]
        public IActionResult ProcessImage(IFormFile uploadedImage, string style)
        {
            if (uploadedImage == null || uploadedImage.Length == 0)
            {
                TempData["ErrorMessage"] = "Lütfen bir fotoğraf yükleyin.";
                return RedirectToAction("Index");
            }

            try
            {
                using var memoryStream = new MemoryStream();
                uploadedImage.CopyTo(memoryStream);
                var imageBytes = memoryStream.ToArray();

                imageBytes = ResizeImage(imageBytes, 800, 800);

                var client = new RestClient("https://hairstyle-changer.p.rapidapi.com/huoshan/facebody/hairstyle");
                var request = new RestRequest
                {
                    Method = Method.Post
                };

                request.AddHeader("x-rapidapi-key", "ac90719073mshe178b61798780c4p1290d3jsn77591b59ca62");
                request.AddHeader("x-rapidapi-host", "hairstyle-changer.p.rapidapi.com");
                request.AddHeader("Content-Type", "multipart/form-data");

                request.AddFile("image_target", imageBytes, uploadedImage.FileName);
                request.AddParameter("hair_type", style);

                RestResponse response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    var responseData= Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);
                    string base64Image = responseData?.data?.image;
                    ViewBag.ProcessedImage = $"data:image/png;base64,{base64Image}";
                    return View("Result");
                }
                else
                {
                    Console.WriteLine($"Hata Mesajı: {response.ErrorMessage}");
                    Console.WriteLine($"Response icerigi: {response.Content}");
                    TempData["ErrorMessage"] = "Resim isleme basarisiz oldu. Hata: " + response.Content;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        public IActionResult Index()
        {
            ViewBag.HairStyles = new Dictionary<int, string>
            {
                { 101, "Kakül (Varsayılan)" },
                { 201, "Uzun Saç" },
                { 301, "Kakül + Uzun Saç" },
                { 401, "Orta Uzunlukta Saç" },
                { 402, "Hafif Saç Artışı" },
                { 403, "Yoğun Saç Artışı" },
                { 502, "Hafif Kıvırcık" },
                { 503, "Yoğun Kıvırcık" },
                { 603, "Kısa Saç" },
                { 801, "Sarı Saç" },
                { 901, "Düz Saç" },
                { 1001, "Yağsız Saç" },
                { 1101, "Saç Çizgisi Dolgusu" },
                { 1201, "Düzgün Saç" },
                { 1301, "Saç Boşluklarını Doldurma" }
            };
            return View();
        }

    }
}
