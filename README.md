## Usage

To use add the following using statement:
    using ColorUtil.Converter

Next, get the dictionary of colorspaces by creating a new instance of the Converter class:
   var converter = new Converter();

Then call the GetColorSpaces method while passing the input string.
   var cs = converter.GetColorSpaces(input);
GetColorSpaces will return a Dictionary<string, object> of colorspaces where string is the colorspace, and the object contains the colorspace properties.

## Colorspaces Included

