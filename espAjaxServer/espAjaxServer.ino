#include <ESP8266WiFi.h>
#define LED     D0 
const char* ssid = "Test";
const char* password = "12345678";
const char* httpheader = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n";
const char* mainpage = "<!doctype html><head><meta name=\"viewport\" content=\"initial-scale=1, maximum-scale=1\"><link rel=\"stylesheet\" href=\"http://code.jquery.com/mobile/1.4.0/jquery.mobile-1.4.0.min.css\" /><script src=\"http://code.jquery.com/jquery-1.9.1.min.js\"></script><script src=\"http://code.jquery.com/mobile/1.4.0/jquery.mobile-1.4.0.min.js\"></script></head><style>h3, h4 {text-align: center;}span {font-weight: bold;}</style><div data-role=\"page\" data-theme=\"b\"><div data-role=\"header\"><div><h3>ESP8266 Web Control</h3></div></div><div data-role=\"content\"><form><p>The button is <span id=\"buttonState\"></span></p><br><select name=\"flip-1\" id=\"flip-1\" data-role=\"slider\" style=\"float: left;\"><option value=\"off\">LED off</option><option value=\"on\">LED on</option></select></form></div> <div data-role=\"footer\"><div><h4>ESP8266</h4></div></div><script type=text/javascript>$( document ).ready(function() {$('#flip-1').change(function() { if($('#flip-1').val()==\"off\"){$.get(\"/LED=OFF\", function(data, status) {});}else{ $.get(\"/LED=ON\", function(data, status) {});}}); });</script></div>";
  long startTime ;
  long elapsedTime ;
WiFiClient client;
//#define LED 13  
WiFiServer server(80);
 
void setup() {
  Serial.begin(115200);
  delay(10);
 
  pinMode(LED, OUTPUT);
  digitalWrite(LED, HIGH);
 
//   Connect to WiFi network
  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
 
  WiFi.begin(ssid, password);
 
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");
 
//   Start the server
  server.begin();
  Serial.println("Server started");
 
 //  Print the IP address
  Serial.print("Use this URL to connect: ");
  Serial.print("http:");
  Serial.print(WiFi.localIP());
  Serial.println("/");
 
}

void sendRespond(String content){
  startTime = millis();
  client.print(httpheader+content);
  client.flush();
  client.stop();
  elapsedTime =   millis() - startTime;
  Serial.print("send respond time: "); 
  Serial.println(elapsedTime);
}

void handleRequest(){

  startTime = millis();
  String request = client.readStringUntil('\r'); 
  // Serial.println(request);
  elapsedTime =   millis() - startTime;
  Serial.print("read the request time: "); 
  Serial.println(elapsedTime);

  if (request.indexOf("/LED=ON") != -1)  {
    digitalWrite(LED, LOW);
    sendRespond("got on request");
  }else
  if (request.indexOf("/LED=OFF") != -1)  {
    digitalWrite(LED, HIGH);
  sendRespond("got on off");
  }else{
    sendRespond(mainpage);
  }
}

 
void loop() {
   long startTime ;
  long elapsedTime ;
//   Check if a client has connected
   client = server.available();
  if (!client) {
    return;
  }
 startTime = millis();
 //  Wait until the client sends some data
  Serial.println("new client");
  while(!client.available()){
    delay(1);
  }
  handleRequest();
  elapsedTime =   millis() - startTime;
  Serial.print("Total time to handle the request: "); 
  Serial.println(elapsedTime);
  Serial.println("Client disonnected");
  Serial.println("");
}
