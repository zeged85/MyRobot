/*
 *  This sketch sends data via HTTP GET requests to data.sparkfun.com service.
 *
 *  You need to get streamId and privateKey at data.sparkfun.com and paste them
 *  below. Or just customize this script to talk to other HTTP servers.
 *
 */

#include <ESP8266WiFi.h>

const char* ssid     = "Test";
const char* password = "12345678";

const char* host = "172.20.10.2"; //set server's known ip
const int httpPort = 1234; //server port
const char* streamId   = "....................";
const char* privateKey = "....................";

String myIPstr = "";
String clientMac = "";


void setup() {
  Serial.begin(115200);
  delay(10);

  // We start by connecting to a WiFi network

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
  Serial.println("IP address: ");
  //resolve ip
  IPAddress myIP = WiFi.localIP();
  myIPstr=IpAddress2String(myIP);
  Serial.println(myIPstr);
  //resolve MAC
 
unsigned char mac[6];
WiFi.macAddress(mac);
clientMac = macToStr(mac);
Serial.println("MAC address: ");
 Serial.println(clientMac);
 Serial.println();

}


int value = 0;


  



void loop() {
  delay(5000);
  ++value;

  Serial.print("connecting to ");
  Serial.println(host);
  
  // Use WiFiClient class to create TCP connections
  WiFiClient client;
  
  if (!client.connect(host, httpPort)) {
    Serial.println("connection failed");
    return;
  }
  
  // We now create a URI for the request

  
  Serial.println("Sending data");
  
  
  // This will send the request to the server

  String line = "Client ip:" + myIPstr + " MAC address: " + clientMac;
  client.println(line);
 
  
  //+ url + " HTTP/1.1\r\n" +
    //           "Host: " + host + "\r\n" + 
    //           "Connection: close\r\n\r\n");
  unsigned long timeout = millis();
  while (client.available() == 0) {
    if (millis() - timeout > 5000) {
      Serial.println(">>> Client Timeout !");
      client.stop();
      return;
    }
  }
  
  // Read all the lines of the reply from server and print them to Serial
  while(client.available()){
    String line = client.readStringUntil('\r'); //BLOCKING
    
    //
    //parse string for commands
    //execute commands
    //client.println("OK"); - send ack
    Serial.print(line); //DEBUG
    //
    //
  }
  
  Serial.println();
  Serial.println("closing connection");
}

String IpAddress2String(const IPAddress& ipAddress)
{
  return String(ipAddress[0]) + String(".") +\
  String(ipAddress[1]) + String(".") +\
  String(ipAddress[2]) + String(".") +\
  String(ipAddress[3])  ; 
}

String macToStr(const uint8_t* mac)
{
  
String result;
  for (int i = 0; i < 6; ++i) {
    result += String(mac[i], 16);
    if (i < 5)
      result += ':';
  }
  return result;
}
