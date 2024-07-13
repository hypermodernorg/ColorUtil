# ColorUtil

ColorUtil is a C# .net 8 library that accepts a colorspace string input, detects the type of color space, and returns a dictionary of various colorspaces along with their properties.


## Usage

To use add the following using statement:

```csharp
using ColorUtil.Converter
```

Next, get the dictionary of colorspaces by creating a new instance of the Converter class:

```csharp
var converter = new Converter();
```

Then call the GetColorSpaces method while passing the input string.

```csharp
var cs = converter.GetColorSpaces(input);
```

The input string should look similar to #000, #000000, rgb(0, 0, 0), hsl(0,0%,0%) and etc.

GetColorSpaces will return a Dictionary<string, object> of colorspaces where string is the colorspace, and the object contains the colorspace properties.

To Get a specific colorspace, you can use the following:

```csharp
var rgb = cs.RGB;
```

And to access the properties of the colorspace, you can use the following:

```csharp   
var r = rgb.R;  
```

## Colorspaces Included

- CMYK
- HEX
- HSL
- HSV
- RGB
- LAB
- RGBA  - *Output only*
- XYZ
- YUV
- YCbCr
- AdobeRGB


### Example contents of cs:

```csharp
{
    "RGB": {
        "Name": "RGB",
        "R": 255,
        "G": 0,
        "B": 0,
        "Code": "#FF0000"
    },
    "HSL": {
        "Name": "HSL",
        "H": 0,
        "S": 100,
        "L": 50,
        "Code": "hsl(0, 100%, 50%)"
    },
    "XYZ": {
        "Name": "XYZ",
        "X": 41.24,
        "Y": 21.26,
        "Z": 1.93,
        "Code": "xyz(41.24, 21.26, 1.93)"
    }
}
```