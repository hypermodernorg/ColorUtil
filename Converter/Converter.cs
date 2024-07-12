using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ColorUtil.Converter.ColorSpaces;

namespace ColorUtil.Converter
{

    // Determine the format of the color.
    public class Converter
    {
   
        public Dictionary<string, Object> GetColorSpaces(string color)
        {
            List<Type> colorSpaceTypes = ColorSpaceTypes(); // List of ColorSpaces
            List<Object> objects = new(); // List of objects in each color space.
            string detectedColorSpace = DetectColorSpace(color, colorSpaceTypes); // Find the color space of the color.
            Dictionary<string, object> colorSpaceDictionary = new(); // Dictionary to hold color space objects

            RGB rgb = new();

            if (detectedColorSpace == "Unknown")
            {
                colorSpaceDictionary["Unknown"] = true;
                return colorSpaceDictionary;
            }
            // Create an instance of the class that matches the color space.
            Type type = Type.GetType("ColorUtil.Converter.ColorSpaces." + detectedColorSpace);
            object instance = Activator.CreateInstance(type);

            // Get the method to convert the color to RGB.
            MethodInfo method = type.GetMethod("To");

            // Convert the color to RGB by running the method in the correct ColorSpace class.
            rgb = (RGB)method.Invoke(instance, new object[] { color });
            colorSpaceDictionary["RGB"] = rgb;
            objects.Add(rgb);

            // Loop through all of the types in the ColorSpaces namespace, except RGB, and convert the RGB color to the other color spaces.
            foreach (var colorSpaceType in colorSpaceTypes)
            {
                if (colorSpaceType.Name != "RGB")
                {
                    object instance2 = Activator.CreateInstance(colorSpaceType);
                    MethodInfo method2 = colorSpaceType.GetMethod("From");
                    object result = method2.Invoke(instance2, new object[] { objects[0]});

                    objects.Add(result);
                    colorSpaceDictionary[colorSpaceType.Name] = result;
                }
            }
            return colorSpaceDictionary;
        }

        public List<Type> ColorSpaceTypes()
        {
            // Get a list of classes in the ColorSpaces namespace.
            string ColorSpacesList = "ColorUtil.Converter.ColorSpaces"; // This is the namespace.

            // Get the assembly containing the types (classes)
            // Assuming the classes are in the current executing assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Get all types in the assembly
            Type[] types = assembly.GetTypes();

            // Filter types to find all classes within the specified namespace
            var classesInNamespace = types
                .Where(type => type.IsClass && type.Namespace == ColorSpacesList)
                .ToList();

            // Print the names of the classes
            foreach (var classType in classesInNamespace)
            {
                //Console.WriteLine(classType.Name);
            }

            return classesInNamespace;
        }

        public string DetectColorSpace(string color, List<Type> csl)
        {
            // Get all the fields for each type
            foreach (var type in csl) {
                FieldInfo[] fields = type.GetFields();
                foreach (var field in fields)
                {
                    Console.WriteLine(field.Name);
                    Console.WriteLine(type.Name);

                    //if the name of the field is Pattern, then check if the color matches the pattern.
                    if (field.Name == "Pattern")
                    {
                        string pattern = field.GetValue(null).ToString();
                        if (Regex.IsMatch(color, pattern))
                        {
                            return type.Name;
                        }
                    }
                }
            }
          
            return "Unknown";
        }
    }
}
