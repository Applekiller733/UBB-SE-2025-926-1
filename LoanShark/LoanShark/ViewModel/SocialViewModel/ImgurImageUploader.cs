// <copyright file="ImgurImageUploader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Storage;

public class ImgurImageUploader
{
    private const string ClientId = "ecde1e79945f70c";

    public static async Task<string?> UploadImageAndGetUrl(StorageFile file)
    {
        if (file == null)
        {
            return null;
        }

        using (var httpClient = new HttpClient())
        {
            byte[] imageBytes;
            using (var stream = await file.OpenStreamForReadAsync())
            {
                imageBytes = new byte[stream.Length];
                await stream.ReadAsync(imageBytes, 0, imageBytes.Length);
            }

            string base64Image = Convert.ToBase64String(imageBytes);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.imgur.com/3/image")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("image", base64Image),
                }),
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Client-ID", ClientId);

            var response = await httpClient.SendAsync(request);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            return result?.data?.link;
        }
    }


    //public static async Task<string?> UploadImageAndGetUrl(StorageFile file)
    //{
    //    if (file == null)
    //    {
    //        Console.WriteLine("No file provided for upload");
    //        return null;
    //    }

    //    try
    //    {
    //        using (var httpClient = new HttpClient())
    //        {
    //            // Read file as stream
    //            using (var stream = await file.OpenStreamForReadAsync())
    //            {
    //                if (stream.Length > 10 * 1024 * 1024) // 10MB limit
    //                {
    //                    Console.WriteLine($"File {file.Name} is too large: {stream.Length} bytes");
    //                    return null;
    //                }

    //                var content = new MultipartFormDataContent();
    //                var fileContent = new StreamContent(stream);
    //                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
    //                content.Add(fileContent, "image", file.Name);

    //                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.imgur.com/3/image")
    //                {
    //                    Content = content
    //                };
    //                request.Headers.Authorization = new AuthenticationHeaderValue("Client-ID", ClientId);

    //                Console.WriteLine($"Uploading image {file.Name} to Imgur");
    //                var response = await httpClient.SendAsync(request);
    //                response.EnsureSuccessStatusCode(); // Throws if not successful

    //                string jsonResponse = await response.Content.ReadAsStringAsync();
    //                Console.WriteLine($"Imgur response: {jsonResponse}");

    //                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResponse);
    //                string? imageUrl = result?.data?.link;
    //                if (string.IsNullOrEmpty(imageUrl))
    //                {
    //                    Console.WriteLine("Failed to get image URL from Imgur response");
    //                    return null;
    //                }

    //                Console.WriteLine($"Image uploaded successfully: {imageUrl}");
    //                return imageUrl;
    //            }
    //        }
    //    }
    //    catch (HttpRequestException ex)
    //    {
    //        Console.WriteLine($"HTTP error uploading image: {ex.Message}");
    //        return null;
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error uploading image: {ex.Message}");
    //        return null;
    //    }
    //}
}
