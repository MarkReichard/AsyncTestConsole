using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// This is the code from
/// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
///  plus some annotations.
/// This is the simplest, most concrete explanation I've seen of 
/// async/await.
/// </summary>
namespace AsyncTestConsole
{
     class Program
     {
          static async Task Main(string[] args)
          {
               //start timing how long the process takes
               Stopwatch watch = new Stopwatch();
               watch.Start();

               Coffee cup = PourCoffee();
               Console.WriteLine("coffee is ready");

               //Start our asynchronous tasks all at once.
               //Don't await results to avoid blocking subsequent tasks.
               //Instead deal with task objects.
               Task<Egg> eggsTask = FryEggsAsync(2);
               Task<Bacon> baconTask = FryBaconAsync(3);
               Task<Toast> toastTask = MakeToastWithButterAndJamAsync(2);

               //create a list of task objects.  We'll remove them as we
               //do console.writelines about each one finishing
               var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };
               //while we have tasks
               while (breakfastTasks.Count > 0)
               {
                    //when any is called when any of our tasks complete
                    Task finishedTask = await Task.WhenAny(breakfastTasks);
                    if (finishedTask == eggsTask)
                    {
                         Console.WriteLine("eggs are ready");
                    }
                    else if (finishedTask == baconTask)
                    {
                         Console.WriteLine("bacon is ready");
                    }
                    else if (finishedTask == toastTask)
                    {
                         Console.WriteLine("toast is ready");
                    }
                    breakfastTasks.Remove(finishedTask);
               }
               Juice oj = PourOJ();
               Console.WriteLine("oj is ready");
               Console.WriteLine("Breakfast is ready!");

               //write how long the process took to screen
               watch.Stop();
               Console.WriteLine("Breakfast took " 
                    + watch.Elapsed.TotalSeconds.ToString()
                    + " seconds.");
          }

          private static Juice PourOJ()
          {
               Console.WriteLine("Pouring orange juice");
               return new Juice();
          }

          private static void ApplyJam(Toast toast) =>
              Console.WriteLine("Putting jam on the toast");

          private static void ApplyButter(Toast toast) =>
              Console.WriteLine("Putting butter on the toast");

          private static async Task<Toast> ToastBreadAsync(int slices)
          {
               for (int slice = 0; slice < slices; slice++)
               {
                    Console.WriteLine("Putting a slice of bread in the toaster");
               }
               Console.WriteLine("Start toasting...");
               //await is what allows the other tasks to continue during delay
               await Task.Delay(3000);
               Console.WriteLine("Remove toast from toaster");

               return new Toast();
          }

          static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
          {
               var toast = await ToastBreadAsync(number);
               ApplyButter(toast);
               ApplyJam(toast);

               return toast;
          }

          private static async Task<Bacon> FryBaconAsync(int slices)
          {
               Console.WriteLine($"putting {slices} slices of bacon in the pan");
               Console.WriteLine("cooking first side of bacon...");
               //await is what allows the other tasks to continue during delay
               await Task.Delay(3000);
               for (int slice = 0; slice < slices; slice++)
               {
                    Console.WriteLine("flipping a slice of bacon");
               }
               Console.WriteLine("cooking the second side of bacon...");
               //await is what allows the other tasks to continue during delay
               await Task.Delay(3000);
               Console.WriteLine("Put bacon on plate");

               return new Bacon();
          }

          private static async Task<Egg> FryEggsAsync(int howMany)
          {
               Console.WriteLine("Warming the egg pan...");
               //await is what allows the other tasks to continue during delay
               await Task.Delay(3000);
               Console.WriteLine($"cracking {howMany} eggs");
               Console.WriteLine("cooking the eggs ...");
               //await is what allows the other tasks to continue during delay
               await Task.Delay(3000);
               Console.WriteLine("Put eggs on plate");

               return new Egg();
          }

          private static Coffee PourCoffee()
          {
               Console.WriteLine("Pouring coffee");
               return new Coffee();
          }
     }
}