#region License

/* SDL2# - C# Wrapper for SDL2
 *
 * Copyright (c) 2013-2020 Ethan Lee.
 *
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from
 * the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 * claim that you wrote the original software. If you use this software in a
 * product, an acknowledgment in the product documentation would be
 * appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source distribution.
 *
 * Ethan "flibitijibibo" Lee <flibitijibibo@flibitijibibo.com>
 *
 */

#endregion

#region Using Statements

using System.Runtime.InteropServices;

#endregion

namespace SDL2 {

    [Flags]
    public enum InitOptions {
        Jpg = 0x00000001,
        Png = 0x00000002,
        Tif = 0x00000004,
        Webp = 0x00000008
    }

    public static partial class Image {

        #region SDL2# Variables

        /* Used by DllImport to load the native library. */
        private const string nativeLibName = "SDL2_image";

        #endregion

        #region SDL_image.h

        /* Similar to the headers, this is the version we're expecting to be
         * running with. You will likely want to check this somewhere in your
         * program!
         */
        public const int ImageMajorVersion = 2;
        public const int ImageMinorVersion = 0;
        public const int PatchLevel = 28;

        /// <summary>
        /// Initialization Flags
        /// </summary>
        [Flags]
        public enum InitOptions {
            Jpg = 0x00000001,
            Png = 0x00000002,
            Tif = 0x00000004,
            Webp = 0x00000008
        }

        /// <summary>
        /// This macro can be used to fill a version structure with the compile-time version of the <see cref="Image"/> library.
        /// </summary>
        public static void ImageVersion(out Version X) {
            X.Major = ImageMajorVersion;
            X.Minor = ImageMinorVersion;
            X.Patch = PatchLevel;
        }

        /// <summary>
        /// Initialize SDL_image.
        /// </summary>
        /// <param name="flags">Initialization flags, OR'd together.</param>
        /// <remarks>
        /// <para>
        /// This function loads dynamic libraries that <see cref="Image"/> needs and prepares them for use.
        /// This must be the first function you call in <see cref="Image"/>, and if it fails, you should not continue with the library.
        /// </para>
        /// <para>
        /// Flags should be one or more values from <see cref="InitOptions"/> OR'd together. It returns the flags that were successfully
        /// initialized, or 0 on failure. More flags may be added in future SDL_image releases.
        /// </para>
        /// <para>
        /// This function may need to load external shared libraries to support various codecs. As a result, it can fail even on
        /// otherwise-reasonable systems if those libraries are missing. This is not limited to rare errors like running out of memory.
        /// </para>
        /// <para>
        /// You may call this function more than once to initialize additional flags. The return value reflects both newly-initialized flags
        /// and any previously-initialized ones. Because of this behavior, it's safe to call this with zero (no flags set) as a way to
        /// query the current initialization state without changing it.
        /// </para>
        /// <para>
        /// Since it returns all previously-initialized flags, do not check for a zero return value to determine an error. Instead,
        /// verify that all required flags are set in the return value.
        /// </para>
        /// <para>
        /// If your app depends on a specific image format, missing support may be fatal. But a general image viewer could continue
        /// operating even if some formats (e.g., WEBP) are unavailable.
        /// </para>
        /// <para>
        /// Unlike other SDL satellite libraries, calls to <see cref="Init(InitOptions)"/> do not stack. A single call to <see cref="Quit"/>
        /// deinitializes everything. It is best practice to have one <see cref="Init(InitOptions)"/> and one <see cref="Quit"/> call
        /// per program, though not strictly required.
        /// </para>
        /// <para>
        /// After initializing <see cref="Image"/>, the app may begin loading images into SDL_Surfaces or SDL_Textures.
        /// </para>
        /// <para>
        /// This function is available since SDL_image 2.0.0.
        /// </para>
        /// </remarks>
        /// <returns>All currently initialized flags.</returns>
        public static int Init(InitOptions flags) {
            // Validate the flags to ensure they are within the expected range
            if (!Enum.IsDefined(flags)) {
                throw new ArgumentException("Invalid initialization flags provided.", nameof(flags));
            }

            // Call the native method and check the result
            int result = INTERNAL_Init(flags);
            if (result == 0) {
                throw new InvalidOperationException("Failed to initialize SDL2_image with the provided flags.");
            }

            return result;
        }

        /// <summary>
        /// This function gets the version of the dynamically linked SDL_image library.
        /// </summary>
        /// <returns>(const SDL_version *) Returns SDL_image version.</returns>
        public static Version LinkedVersion() {
            Version result;
            nint result_ptr = INTERNAL_IMG_Linked_Version();
            result = Marshal.PtrToStructure<Version>(result_ptr);
            return result;
        }

        /// <summary>
        /// Load an image from a filesystem path into a software surface.
        /// </summary>
        /// <param name="file">A path on the filesystem to load an image from.</param>
        /// <remarks>
        /// <para>
        /// An SDL_Surface is a buffer of pixels in memory accessible by the CPU.
        /// Use this if you plan to hand the data to something else or manipulate it further in code.
        /// </para>
        /// <para>
        /// There are no guarantees about what format the new SDL_Surface data will be; in many cases, <see cref="Image"/> will attempt to supply a
        /// surface that exactly matches the provided image, but in others it might have to convert
        /// (either because the image is in a format that SDL doesn't directly support or because it's compressed data
        /// that could reasonably uncompress to various formats and <see cref="Image"/> had to pick one).
        /// </para>
        /// <para>You can inspect an SDL_Surface for its specifics, and use <see cref="SDL.ConvertSurface(nint, nint, uint)"/> to then migrate to any supported format.
        /// If the image format supports a transparent pixel, SDL will set the colorkey for the surface.
        /// </para>
        /// <para>
        /// You can enable RLE acceleration on the surface afterwards by calling: <see cref="SDL.SetColorKey(nint, int, uint)"/>
        /// There is a separate function to read files from an SDL_RWops, if you need an i/o abstraction to provide data from anywhere
        /// instead of a simple filesystem read; that function is <see cref="LoadRw(nint, bool)"/>.
        /// </para>
        /// <para>
        /// If you are using SDL's 2D rendering API, there is an equivalent call to load images directly into an SDL_Texture for use
        /// by the GPU without using a software surface: call <see cref="LoadTexture(nint, string)"/> instead.
        /// </para>
        /// <para>
        /// When done with the returned surface, the app should dispose of it with a call to <see cref="SDL.FreeSurface(nint)"/>.
        /// </para>
        /// This function is available since SDL_image 2.0.0.
        /// </remarks>
        /// <returns>(SDL_Surface *) Returns a new SDL surface, or NULL on error.</returns>
        public static nint Load(string file) {
            if (!File.Exists(file)) {
                throw new FileNotFoundException("File not found", file);
            }
            return INTERNAL_IMG_Load(SDL.UTF8_ToNative(file));
        }

        /// <summary>
        /// Load an image from an SDL data source into a software surface.
        /// </summary>
        /// <param name="src">an SDL_RWops that data will be read from. nint refers to a SDL_RWops *</param>
        /// <param name="freeSrc">True whether to free the <paramref name="src"/> or not</param>
        /// <remarks>
        /// <para>
        /// A <see cref="Surface"/> is a buffer of pixels in memory accessible by the CPU.
        /// Use this if you plan to hand the data to something else or manipulate it further in code.
        /// </para>
        /// <para>
        /// There are no guarantees about what format the new <see cref="Surface"/> data will be; in many cases, <see cref="Image"/> will attempt
        /// to supply a surface that exactly matches the provided image, but in others it might have to convert
        /// (either because the image is in a format that SDL doesn't directly support or because it's compressed data that could
        /// reasonably uncompress to various formats and <see cref="Image"/> had to pick one). You can inspect a <see cref="Surface"/> for its specifics,
        /// and use <see cref="SDL.ConvertSurface(nint, nint, uint)"/> to then migrate to any supported format.
        /// </para>
        /// <para>
        /// If the image format supports a transparent pixel, SDL will set the colorkey for the surface.
        /// You can enable RLE acceleration on the surface afterwards by calling: <see cref="SDL.SetColorKey(nint, int, uint)"/>.
        /// </para>
        /// <para>
        /// If freesrc is non-zero, the RWops will be closed before returning, whether this function succeeds or not.
        /// <see cref="Image"/> reads everything it needs from the RWops during this call in any case.
        /// </para>
        /// <para>
        /// There is a separate function to read files from disk without having to deal with SDL_RWops:
        /// <see cref="Load(string)"/> will call this function and manage those details for you, determining the file type from the filename's extension.
        /// </para>
        /// <para>
        /// There is also <see cref="LoadTypedRw(nint, int, string)"/>, which is equivalent to this function except a file extension (like "BMP", "JPG", etc)
        /// can be specified, in case <see cref="Image"/> cannot autodetect the file format.
        /// </para>
        /// <para>
        /// If you are using SDL's 2D rendering API, there is an equivalent call to load images directly into a Texture for use by the GPU without using
        /// a software surface: call <see cref="LoadTextureRw(nint, nint, bool)"/> instead.
        /// When done with the returned surface, the app should dispose of it with a call to <see cref="SDL.FreeSurface(nint)"/>.
        /// </para>
        /// This function is available since SDL_image 2.0.0.
        /// </remarks>
        /// <returns>(SDL_Surface *) Returns a new SDL surface, or NULL on error.</returns>
        public static nint LoadRw(nint src, bool freeSrc) {
            // Validate the source pointer to ensure it is not null
            if (src == nint.Zero) {
                throw new ArgumentNullException(nameof(src), "Source pointer cannot be null.");
            }

            // Call the internal native method and handle any potential errors
            nint result = INTERNAL_LoadRw(src, freeSrc ? 1 : 0);
            if (result == nint.Zero) {
                throw new InvalidOperationException("Failed to load RW. SDL2_image returned a null pointer.");
            }

            return result;
        }

        /// <summary>
        /// Load an image from a filesystem path into a GPU texture.
        /// </summary>
        /// <param name="renderer">the SDL_Renderer to use to create the GPU texture. nint refers to a SDL_Renderer *</param>
        /// <param name="file">a path on the filesystem to load an image from.</param>
        /// <remarks>
        /// <para>
        /// An SDL_Texture represents an image in GPU memory, usable by SDL's 2D Render API.
        /// This can be significantly more efficient than using a CPU-bound <see cref="Surface"/> if you don't need to manipulate the image directly after loading it.
        /// </para>
        /// <para>
        /// If the loaded image has transparency or a colorkey, a texture with an alpha channel will be created. Otherwise, <see cref="Image"/> will attempt to create
        /// an SDL_Texture in the most format that most reasonably represents the image data (but in many cases, this will just end up being 32-bit RGB or 32-bit RGBA).
        /// </para>
        /// <para>
        /// There is a separate function to read files from an SDL_RWops, if you need an i/o abstraction to provide data from anywhere instead of a simple filesystem read; 
        /// that function is <see cref="LoadTextureRw(nint, nint, bool)"/>.
        /// If you would rather decode an image to an <see cref="Surface"/> (a buffer of pixels in CPU memory), call <see cref="Load(string)"/> instead.
        /// When done with the returned texture, the app should dispose of it with a call to <see cref="SDL.DestroyTexture(nint)"/>.
        /// </para>
        /// This function is available since SDL_image 2.0.0.
        /// </remarks>
        /// <returns>(SDL_Texture *) Returns a new texture, or NULL on error.</returns>
        public static nint LoadTexture(
            nint renderer,
            string file
        ) {
            return INTERNAL_IMG_LoadTexture(
                renderer,
                SDL.UTF8_ToNative(file)
            );
        }

        /// <summary>
        /// Load an image from an SDL data source into a GPU texture.
        /// </summary>
        /// <param name="renderer">the SDL_Renderer to use to create the GPU texture. nint refers to a SDL_Renderer *</param>
        /// <param name="src">an SDL_RWops that data will be read from. nint refers to a SDL_RWops *</param>
        /// <remarks>
        /// <para>
        /// An SDL_Texture represents an image in GPU memory, usable by SDL's 2D Render API. 
        /// This can be significantly more efficient than using a CPU-bound <see cref="Surface"/> if you don't need to manipulate the image directly after loading it.
        /// </para>
        /// <para>
        /// If the loaded image has transparency or a colorkey, a texture with an alpha channel will be created. Otherwise, <see cref="Image"/> will attempt to create 
        /// an SDL_Texture in the most format that most reasonably represents the image data (but in many cases, this will just end up being 32-bit RGB or 32-bit RGBA).
        /// </para>
        /// <para>
        /// If freesrc is true, the RWops will be closed before returning, whether this function succeeds or not. <see cref="Image"/> reads everything it 
        /// needs from the RWops during this call in any case. There is a separate function to read files from disk without having to deal
        /// with SDL_RWops: <see cref="LoadTexture(nint, string)"/> will call this function and manage those details for you, determining the file type from the filename's extension.
        /// </para>
        /// <para>
        /// There is also <see cref="LoadTextureTypedRw(nint, nint, int, string)"/>, which is equivalent to this function except a file extension (like "BMP", "JPG", etc) 
        /// can be specified, in case <see cref="Image"/> cannot autodetect the file format.
        /// If you would rather decode an image to an <see cref="Surface"/> (a buffer of pixels in CPU memory), call <see cref="Load(string)"/> instead.
        /// When done with the returned texture, the app should dispose of it with a call to <see cref="SDL.DestroyTexture(nint)"/>.
        /// </para>
        /// This function is available since SDL_image 2.0.0.
        /// </remarks>
        /// <returns>(SDL_Texture *) Returns a new texture, or NULL on error.</returns>
        public static nint LoadTextureRw(nint renderer, nint src, bool freeSrc) {
            return INTERNAL_LoadTextureRw(renderer, src, freeSrc ? 1 : 0);
        }

        /// <summary>
        /// Load an image from an SDL data source into a GPU texture.
        /// </summary>
        /// <param name="renderer">the SDL_Renderer to use to create the GPU texture. nint refers to a SDL_Renderer *</param>
        /// <param name="src">an SDL_RWops that data will be read from. nint refers to a SDL_RWops * </param>
        /// <param name="type">a filename extension that represent this data ("BMP", "GIF", "PNG", etc).</param>
        /// <remarks>
        /// <para>
        /// An SDL_Texture represents an image in GPU memory, usable by SDL's 2D Render API. This can be significantly more efficient 
        /// than using a CPU-bound SDL_Surface if you don't need to manipulate the image directly after loading it.
        /// </para>
        /// <para>
        /// If the loaded image has transparency or a colorkey, a texture with an alpha channel will be created. Otherwise, <see cref="Image"/> will attempt to 
        /// create an SDL_Texture in the most format that most reasonably represents the image data (but in many cases, this will just end up being 32-bit RGB or 32-bit RGBA).
        /// If freesrc is true, the RWops will be closed before returning, whether this function succeeds or not. <see cref="Image"/> reads everything it needs from the RWops during this call in any case.
        /// </para>
        /// <para>
        /// Even though this function accepts a file type, <see cref="Image"/> may still try other decoders that are capable of detecting file type from the contents of the image data, 
        /// but may rely on the caller-provided type string for formats that it cannot autodetect.
        /// If type is NULL, <see cref="Image"/> will rely solely on its ability to guess the format.
        /// </para>
        /// <para>
        /// There is a separate function to read files from disk without having to deal with SDL_RWops: <see cref="LoadTexture(nint, string)"/> will call this function and manage those details for you, 
        /// determining the file type from the filename's extension.
        /// There is also <see cref="LoadTextureRw(nint, nint, bool)"/>, which is equivalent to this function except that it will rely on <see cref="Image"/> to
        /// determine what type of data it is loading, much like passing a NULL for type.
        /// </para>
        /// <para>
        /// If you would rather decode an image to an <see cref="Surface"/> (a buffer of pixels in CPU memory), call <see cref="LoadTypedRw(nint, bool, string)"/> instead.
        /// When done with the returned texture, the app should dispose of it with a call to <see cref="SDL.DestroyTexture(nint)"/>.
        /// </para>
        /// This function is available since SDL_image 2.0.0.
        /// </remarks>
        /// <returns>(SDL_Texture *) Returns a new texture, or NULL on error.</returns>
        public static nint LoadTextureTypedRw(
            nint renderer,
            nint src,
            int freeSrc,
            string? type
        ) {
            return INTERNAL_IMG_LoadTextureTyped_RW(
                renderer,
                src,
                freeSrc,
                SDL.UTF8_ToNative(type!)
            );
        }

        /// <summary>
        /// Load an image from an SDL data source into a software surface.
        /// </summary>
        /// <param name="src">an SDL_RWops that data will be read from. nint refers to a SDL_RWops * </param>
        /// <param name="type">a filename extension that represent this data ("BMP", "GIF", "PNG", etc).</param>
        /// <param name="freeSrc">True whether to free the <paramref name="src"/> or not.</param>
        /// <remarks>
        /// <para>
        /// A <see cref="Surface"/> is a buffer of pixels in memory accessible by the CPU.
        /// Use this if you plan to hand the data to something else or manipulate it further in code.
        /// </para>
        /// <para>
        /// There are no guarantees about what format the new <see cref="Surface"/> data will be; in many cases, <see cref="Image"/> will attempt to supply
        /// a surface that exactly matches the provided image, but in others it might have to convert
        /// (either because the image is in a format that SDL doesn't directly support or because it's compressed data that
        /// could reasonably uncompress to various formats and <see cref="Image"/> had to pick one).
        /// </para>
        /// <para>
        /// You can inspect an <see cref="Surface"/> for its specifics, and use <see cref="SDL.ConvertSurface(nint, nint, uint)"/> to then migrate to any supported format.
        /// If the image format supports a transparent pixel, SDL will set the colorkey for the surface.
        /// </para>
        /// <para>
        /// You can enable RLE acceleration on the surface afterwards by calling: <see cref="SDL.SetColorKey(nint, int, uint)"/>
        /// If freesrc is true, the RWops will be closed before returning, whether this function succeeds or not.
        /// </para>
        /// <para>
        /// <see cref="Image"/> reads everything it needs from the RWops during this call in any case.
        /// Even though this function accepts a file type, <see cref="Image"/> may still try other decoders that are capable of
        /// detecting file type from the contents of the image data, but may rely on the caller-provided type string for formats that it cannot autodetect.
        /// If type is NULL, <see cref="Image"/> will rely solely on its ability to guess the format.
        /// </para>
        /// <para>
        /// There is a separate function to read files from disk without having to deal with SDL_RWops: <see cref="Load(string)"/> will call this function
        /// and manage those details for you, determining the file type from the filename's extension.
        /// There is also <see cref="LoadRw(nint, bool)"/> which is equivalent to this function except that it will rely on <see cref="Image"/> to determine what type of data it is loading, much like passing a NULL for type.
        /// </para>
        /// <para>
        /// If you are using SDL's 2D rendering API, there is an equivalent call to load images directly into an SDL_Texture for use by the GPU without using a software surface: callIMG_LoadTextureTyped_RW() instead.
        /// When done with the returned surface, the app should dispose of it with a call to SDL_FreeSurface().
        /// </para>
        /// This function is available since SDL_image 2.0.0.
        /// </remarks>
        /// <returns>(SDL_Surface *) Returns a new SDL surface, or NULL on error.</returns>
        public static nint LoadTypedRw(
            nint src,
            bool freeSrc,
            string? type
        ) {
            return INTERNAL_IMG_LoadTyped_RW(
                src,
                freeSrc ? 1 : 0,
                SDL.UTF8_ToNative(type!)
            );
        }

        /// <summary>
        /// DeInitialize <see cref="Image"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// This should be the last function you call in <see cref="Image"/>, after freeing all other resources.
        /// This will unload any shared libraries it is using for various codecs.
        /// </para>
        /// <para>
        /// After this call, a call to Init(0) will return 0 (no codecs loaded).
        /// You can safely call <see cref="Init(InitOptions)"/> to reload various codec support after this call.
        /// </para>
        /// <para>
        /// Unlike other SDL satellite libraries, calls to <see cref="Init(InitOptions)"/> do not stack; a single call to <see cref="Quit"/>
        /// will deinitialize everything and does not have to be paired with a matching <see cref="Init(InitOptions)"/> call.
        /// </para>
        /// <para>
        /// For that reason, it's considered best practices to have a single <see cref="Init(InitOptions)"/> and <see cref="Quit"/> call in your program.
        /// While this isn't required, be aware of the risks of deviating from that behavior.
        /// </para>
        /// </remarks>
        public static void Quit() {
            INTERNAL_Quit();
        }

        /// <summary>
        /// Load an XPM image from a memory array.
        /// </summary>
        /// <param name="xpm">a null-terminated array of strings that comprise XPM data.</param>
        /// <remarks>
        /// <para>
        /// The returned <see cref="Surface"/> will be an 8bpp indexed surface, if possible, otherwise it will be 32bpp.
        /// If you always want 32-bit data, use <see cref="ReadXPMFromArrayToRGB888(string[])"/> instead.
        /// When done with the returned surface, the app should dispose of it with a call to <see cref="SDL.FreeSurface(nint)"/>.
        /// </para>
        /// This function is available since SDL_image 2.0.0.
        /// </remarks>
        /// <returns>(SDL_Surface *) Returns a new SDL surface, or NULL on error.</returns>
        public static nint ReadXPMFromArray(string[] xpm) {
            // Validate the input array to ensure it is not null or empty
            if (xpm == null || xpm.Length == 0) {
                throw new ArgumentException("XPM array cannot be null or empty.", nameof(xpm));
            }

            // Call the internal native method
            nint result = INTERNAL_ReadXPMFromArray(xpm);

            // Check if the result is a null pointer, indicating an error
            if (result == nint.Zero) {
                throw new InvalidOperationException("Failed to read XPM from array. SDL2_image returned a null pointer.");
            }

            return result;
        }

        /// <summary>
        /// Load an XPM image from a memory array.
        /// </summary>
        /// <param name="xpm">a null-terminated array of strings that comprise XPM data.</param>
        /// <remarks>
        /// <para>
        /// The returned surface will always be a 32-bit RGB surface. If you want 8-bit indexed colors (and the XPM data allows it), use <see cref="ReadXPMFromArray(string[])"/> instead.
        /// When done with the returned surface, the app should dispose of it with a call to <see cref="SDL.FreeSurface(nint)"/>.
        /// </para>
        /// This function is available since SDL_image 2.6.0.
        /// </remarks>
        /// <returns>(SDL_Surface *) Returns a new SDL surface, or NULL on error.</returns>
        public static nint ReadXPMFromArrayToRGB888(string[] xpm) {
            // Validate the input array to ensure it is not null or empty
            if (xpm == null || xpm.Length == 0) {
                throw new ArgumentException("XPM array cannot be null or empty.", nameof(xpm));
            }

            // Call the internal native method
            nint result = INTERNAL_ReadXPMFromArrayToRGB888(xpm);

            // Check if the result is a null pointer, indicating an error
            if (result == nint.Zero) {
                throw new InvalidOperationException("Failed to read XPM from array to RGB888. SDL2_image returned a null pointer.");
            }

            return result;
        }

        /// <summary>
        /// Save an SDL_Surface into a JPEG image file.
        /// </summary>
        /// <param name="surface">the SDL surface to save. nint refers to a SDL_Surface *</param>
        /// <param name="file">path on the filesystem to write new file to.</param>
        /// <remarks>
        /// <para>
        /// If the file already exists, it will be overwritten.
        /// </para>
        /// This function is available since SDL_image 2.0.2.
        /// </remarks>
        /// <returns>(int) Returns 0 if successful, -1 on error.</returns>
        public static int SaveJpg(nint surface, string file, int quality) {
            return INTERNAL_IMG_SaveJPG(
                surface,
                SDL.UTF8_ToNative(file),
                quality
            );
        }

        /// <summary>
        /// Save an SDL_Surface into JPEG image data, via an SDL_RWops.
        /// </summary>
        /// <param name="surface">the SDL surface to save. nint refers to a SDL_Surface *</param>
        /// <param name="dst">the SDL_RWops to save the image data to. nint refers to a SDL_RWops *</param>
        /// <param name="freeDst">True whether to free the destination after saving</param>
        /// <param name="quality">The quality of the image. [0;33] Low, [34;66] Medium, [67;100] High</param>
        /// <remarks>
        /// <para>
        /// If you just want to save to a filename, you can use <see cref="SaveJpg(nint, string, int)"/> instead.
        /// </para>
        /// This function is available since SDL_image 2.0.2.
        /// </remarks>
        /// <returns>(int) Returns 0 if successful, -1 on error.</returns>
        public static int SaveJpgRw(nint surface, nint dst, bool freeDst, int quality) {
            // Validate the quality parameter to ensure it is within the acceptable range (e.g., 0-100)
            if (quality < 0 || quality > 100) {
                throw new ArgumentOutOfRangeException(nameof(quality), "Quality must be between 0 and 100.");
            }

            // Validate the surface and destination pointers to ensure they are not null
            if (surface == nint.Zero) {
                throw new ArgumentNullException(nameof(surface), "Surface pointer cannot be null.");
            }

            if (dst == nint.Zero) {
                throw new ArgumentNullException(nameof(dst), "Destination pointer cannot be null.");
            }

            // Call the internal native method and handle any potential errors
            int result = INTERNAL_SaveJpgRw(surface, dst, freeDst ? 1 : 0, quality);
            if (result != 0) {
                throw new InvalidOperationException($"Failed to save JPG. SDL2_image returned error code: {result}");
            }

            return result;
        }


        /// <summary>
        /// Save an SDL_Surface into a PNG image file.
        /// </summary>
        /// <param name="surface">the SDL surface to save. nint refers to a SDL_Surface *</param>
        /// <param name="file">path on the filesystem to write new file to.</param>
        /// <remarks>
        /// <para>
        /// If the file already exists, it will be overwritten.
        /// </para>
        /// This function is available since SDL_image 2.0.0.
        /// </remarks>
        /// <returns>(int) Returns 0 if successful, -1 on error.</returns>
        public static int SavePng(nint surface, string file) {
            return INTERNAL_IMG_SavePNG(
                surface,
                SDL.UTF8_ToNative(file)
            );
        }

        /// <summary>
        /// Save an SDL_Surface into PNG image data, via an SDL_RWops.
        /// </summary>
        /// <param name="surface">the SDL surface to save. nint refers to a SDL_Surface *</param>
        /// <param name="dst">the SDL_RWops to save the image data to. nint refers to a SDL_RWops *</param>
        /// <param name="freeDst">True to free the destination after saving</param>
        /// <remarks>
        /// <para>
        /// If you just want to save to a filename, you can use <see cref="SavePng(nint, string)"/> instead.
        /// </para>
        /// This function is available since SDL_image 2.0.0.
        /// </remarks>
        /// <returns>(int) Returns 0 if successful, -1 on error.</returns>
        public static int SavePngRw(nint surface, nint dst, bool freeDst) {
            // Validate the surface pointer to ensure it is not null
            if (surface == nint.Zero) {
                throw new ArgumentNullException(nameof(surface), "Surface pointer cannot be null.");
            }

            // Validate the destination pointer to ensure it is not null
            if (dst == nint.Zero) {
                throw new ArgumentNullException(nameof(dst), "Destination pointer cannot be null.");
            }

            // Call the internal native method and handle any potential errors
            int result = INTERNAL_SavePngRw(surface, dst, freeDst ? 1 : 0);
            if (result != 0) {
                throw new InvalidOperationException($"Failed to save PNG. SDL2_image returned error code: {result}");
            }

            return result;
        }

        [LibraryImport(nativeLibName, EntryPoint = "IMG_Linked_Version")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial nint INTERNAL_IMG_Linked_Version();
        [LibraryImport(nativeLibName, EntryPoint = "IMG_Load")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial nint INTERNAL_IMG_Load(byte[] file);

        [LibraryImport(nativeLibName, EntryPoint = "IMG_LoadTexture")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial nint INTERNAL_IMG_LoadTexture(
            nint renderer,
            byte[] file
        );

        [LibraryImport(nativeLibName, EntryPoint = "IMG_LoadTextureTyped_RW")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial nint INTERNAL_IMG_LoadTextureTyped_RW(
            nint renderer,
            nint src,
            int freeSrc,
            byte[] type
        );

        [LibraryImport(nativeLibName, EntryPoint = "IMG_LoadTyped_RW")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial nint INTERNAL_IMG_LoadTyped_RW(
            nint src,
            int freeSrc,
            byte[] type
        );

        [LibraryImport(nativeLibName, EntryPoint = "IMG_SaveJPG")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial int INTERNAL_IMG_SaveJPG(
            nint surface,
            byte[] file,
            int quality
        );

        [LibraryImport(nativeLibName, EntryPoint = "IMG_SavePNG")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial int INTERNAL_IMG_SavePNG(
            nint surface,
            byte[] file
        );

        [LibraryImport(nativeLibName, EntryPoint = "IMG_Init")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial int INTERNAL_Init(InitOptions flags);
        [LibraryImport(nativeLibName, EntryPoint = "IMG_Load_RW")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial nint INTERNAL_LoadRw(
            nint src,
            int freeSrc
        );

        [LibraryImport(nativeLibName, EntryPoint = "IMG_LoadTexture_RW")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial nint INTERNAL_LoadTextureRw(
            nint renderer,
            nint src,
            int freeSrc
        );

        [LibraryImport(nativeLibName, EntryPoint = "IMG_Quit")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial void INTERNAL_Quit();


        [LibraryImport(nativeLibName, EntryPoint = "IMG_ReadXPMFromArray")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial nint INTERNAL_ReadXPMFromArray(
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]
            string[] xpm
        );

        [LibraryImport(nativeLibName, EntryPoint = "IMG_ReadXPMFromArrayToRGB888")]
        [UnmanagedCallConv (CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial nint INTERNAL_ReadXPMFromArrayToRGB888(
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]
            string[] xpm);

        [LibraryImport(nativeLibName, EntryPoint = "IMG_SaveJPG_RW")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial int INTERNAL_SaveJpgRw(
            nint surface,
            nint dst,
            int freeDst,
            int quality
        );

        [LibraryImport(nativeLibName, EntryPoint = "IMG_SavePNG_RW")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial int INTERNAL_SavePngRw(
            nint surface,
            nint dst,
            int freeDst
        );
        #endregion
    }
}