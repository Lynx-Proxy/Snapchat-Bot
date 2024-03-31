using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Text.Json;
using System.Runtime.InteropServices;
using OpenQA.Selenium.DevTools;


namespace ASB //ASB - Automated Snap Bot
{
    internal class Program
    {
        public class Info {
            public string Username {get; set;}
            public string Password {get; set;}
            public string UsePhoneNumber {get; set;}
            public string PhoneNumber {get; set;}
            public string Target {get; set;}
        }

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, uint type);


        static async Task Main(string[] args)
        {
            Console.Title = "Snapchat Bot";
            Console.WriteLine("Reading Json file");

            Info _info = new Info();

            if(!File.Exists(@"Keys.json")){
                using (StreamWriter sw = new StreamWriter(@"./Keys.json")){
                    sw.Write(JsonSerializer.Serialize(_info));
                    sw.Close();
                }
                Console.WriteLine("Configure newly created config file");
                Environment.Exit(0);
            }

            var read = File.ReadAllText(@"Keys.json");
            var info = JsonSerializer.Deserialize<Info>(read);

            Console.WriteLine("Starting driver");

            var driver = new ChromeDriver();

            async void waitandclick(string xpath, string? btn){
                bool check = false;
                if(btn != null){
                    Console.WriteLine($"Waiting for {btn}");
                }
                while(!check){
                    //await Task.Delay(500);
                    try{
                        //Console.WriteLine("test");
                        driver.FindElement(By.XPath(xpath)).Click();
                        check = true;
                        if(btn != null){
                            Console.WriteLine($"Found {btn}");
                        }
                    }
                    catch(NoSuchElementException){}
                    catch(StaleElementReferenceException){}
                    catch(ElementClickInterceptedException){}
                    catch(ElementNotInteractableException){}
                    catch(NullReferenceException){}
                }
            }

            async void click(string xpath, string? btn){
                
                    //await Task.Delay(500);
                    try{
                        //Console.WriteLine("test");
                        driver.FindElement(By.XPath(xpath)).Click();

                        Console.WriteLine($"clicked {btn}");

                    }
                    catch(NoSuchElementException){}
                    catch(StaleElementReferenceException){}
                    catch(ElementClickInterceptedException){}
                    catch(ElementNotInteractableException){}
                    catch(NullReferenceException){}
                    catch(WebDriverException){}
            }

            void waitandtype(string xpath, string text){
                bool check = false;
                while(!check){
                    try{
                        //Console.WriteLine(000);
                        driver.FindElement(By.XPath(xpath)).SendKeys(text);
                        check = true;
                    }
                    catch(NoSuchElementException){}
                }
            }
            
            try{

                driver.Navigate().GoToUrl("https://web.snapchat.com");
                //Console.WriteLine(driver.CurrentWindowHandle);
            }
            catch(Exception e){
                driver.Close();
                Console.WriteLine(e);
            }

            //Logging in
            waitandclick("/html/body/div/div/main/div[3]/div[1]/div/button[1]", null);
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            driver.SwitchTo().Window(driver.WindowHandles[0]).Close();
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            
            switch(info.UsePhoneNumber.ToLower()){

                case "no":
                    //Console.WriteLine(driver.CurrentWindowHandle);
                    await Task.Delay(3000);
                    waitandtype("/html/body/div[2]/div/div/div[3]/article/div[1]/div[3]/form/div[1]/input", info.Username);
                    await Task.Delay(3000);
                    waitandclick("/html/body/div[2]/div/div/div[3]/article/div[1]/div[3]/form/div[3]/button", null);

                break;
                
                case "yes":
                    //Not tested
                    Console.WriteLine(driver.CurrentWindowHandle);
                    try{
                        driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[3]/article/div[1]/div[3]/form/div[1]/div/a")).Click();
                        SelectElement region = new SelectElement(driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[3]/article/div[1]/div[3]/form/div[1]/div[1]/div/div/select")));
                        region.SelectByValue("🇬🇭 GH +233");
                        driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[3]/article/div[1]/div[3]/form/div[1]/div[2]/div/input")).SendKeys(info.PhoneNumber);
                        driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[3]/article/div[1]/div[3]/form/div[4]/button")).Click();
                    }
                    catch(Exception e){
                        Console.WriteLine(e);
                    }

                break;

            }

            await Task.Delay(5000);
            /*Console.WriteLine(driver.FindElement(By.XPath("/html/body/div/div/div[1]")));
            if(driver.FindElement(By.XPath("/html/body/div/div/div[1]")) != null){
                bool captcha = true;
                MessageBox((IntPtr)0,"Bot check found. Manually vrify", "Verification Detetcted", (uint)0x00000000L);
                while(captcha){
                    if(driver.FindElement(By.XPath("/html/body/div/div/div[1]")) == null){  /html/body/main/div[1]/div[3]/div/div/div/div[2]/div[1]/div/div/div/div/div[1]/div/form/div/ul/li[7]/div
                        Console.WriteLine("1");
                        captcha = false;
                    }
                }
            }
            */
            await Task.Delay(5000);

            waitandtype("/html/body/div[2]/div/div/div[3]/article/div/form/div[1]/div/input", info.Password);
            //Console.WriteLine("here1");
            await Task.Delay(3000);
            waitandclick("/html/body/div[2]/div/div/div[3]/article/div/form/div[3]/button",null);
            //Console.WriteLine("here2");
            await Task.Delay(3000);

            waitandclick("/html/body/main/div[1]/div[2]/div/div/div[4]/div[2]/button[1]",null); 
            //Console.WriteLine("here3");
            
            await Task.Delay(2000);
            waitandclick("/html/body/main/div[1]/div[3]/div/div/div/div[2]/div[1]/div/div/div/div/div",null);
            //Console.WriteLine("here4");
            await Task.Delay(1000);
            waitandclick("/html/body/main/div[1]/div[3]/div/div/div/div[2]/div[1]/div[1]/div/div/div[4]/div[2]/button",null);
            //Console.WriteLine("here5");

            MessageBox((IntPtr)0,"Permission request", "Allow microphone and camera permissions", (uint)0x00000000L);

            switch(info.Target.ToLower()){
                case "best friends":
                //Console.WriteLine("here6");
                    waitandclick("/html/body/main/div[1]/div[3]/div/div/div/div[2]/div[1]/div/div/div/div/div","Camera button");
                    while(true){
                        //Console.WriteLine("here7");
                        //Console.WriteLine("here8");
                        click("/html/body/main/div[1]/div[3]/div/div/div/div[2]/div[1]/div/div/div/div/div/div[2]/div/div/div[1]/button[1]","Take picture button");
                        //Console.WriteLine("here9");
                        click("/html/body/main/div[1]/div[3]/div/div/div/div[2]/div[1]/div/div/div/div/div[2]/div[2]/button[2]","Send button");

                        for(int i = 1; i <= 7; i++ ){
                            click($"/html/body/main/div[1]/div[3]/div/div/div/div[2]/div[1]/div/div/div/div/div[1]/div/form/div/ul/ul/li[" + i + "]/div","Friends button");
                        }

                        click("/html/body/main/div[1]/div[3]/div/div/div/div[2]/div[1]/div/div/div/div/div[1]/div/form/div[2]/button", "Send button");
                    }

                case "":
                    Console.WriteLine("Fill in the Target space with either Best Friends or All, Clossing in 10 secons");
                    await Task.Delay(10000);
                    driver.Close();
                    Environment.Exit(0);

                break;
                case null:
                    Console.WriteLine("Fill in the Target space with either Best Friends or All, Clossing in 10 secons");
                    await Task.Delay(10000);
                    driver.Close();
                    Environment.Exit(0);
                break;
                //case "All"

            }
            
        }
    }
}