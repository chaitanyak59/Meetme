using System;
using API.Helpers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services;

public class PhotoService : IPhotoService
{
    private CloudinarySettings _options;
    private readonly Cloudinary _Cloudinary;
    public PhotoService(IOptions<CloudinarySettings> options)
    {
        _options = options.Value;
        var _CloudAccount = new Account(_options.CloudName, _options.ApiKey, _options.ApiSecret);
        _Cloudinary = new Cloudinary(_CloudAccount);
    }

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation()
                            .Width(512)
                            .Height(512)
                            .Crop("fill")
                            .Gravity("face"),

                Folder = "Meetme/profiles",
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };
            ImageUploadResult? uploadResult = await _Cloudinary.UploadAsync(uploadParams);
            return uploadResult;
        }
        else
        {
            throw new Exception("[Cloudinary Service] File missing");
        }
    }

    public Task<DeletionResult> DeletePhotoAsync(string publicID)
    {
        throw new NotImplementedException();
    }
}
