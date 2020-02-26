int DialSampleDelay = 2;           //How long between samples of the dial
unsigned long DialTime = 0;        //How long since the last toggle of the dial pin
bool DialState = false;            //The current state of the dial
int CurrentDialNumber = 0;         // The current number being dialled
int DialTimeout = 200;             // How long from the last dialevent until the number is sent over serial
unsigned long DialTimeoutTime = 0; // How long until the phone sends the number that was dialled
int DialPin = 2;                   // The digital input that the dial is connected to

int CradleSampleDelay = 2;         // How long between samples of the cradle
unsigned long CradleTime = 0;      // How long since the last toggle of the cradle pin
bool CradleState = false;          // The current state of the cradle
int CradlePin = 3;                 // The digital input that the cradle is connected to

void setup() {
  pinMode(DialPin,   INPUT_PULLUP); // Set the dial pin to be an input pin with an internal pullup resistor
  pinMode(CradlePin, INPUT_PULLUP); // Set the cradle pin to be an input pin with an internal pullup resistor
  Serial.begin(9600);               // Start the serial console
}

void loop() {
  if(millis() - DialTime >= DialSampleDelay){ // If we have exceeded the time between dial samples
    if(digitalRead(DialPin) != DialState){    // And the dial state has changed
      DialState = digitalRead(DialPin);       // Update the state of the dial
      
      if(DialState){                          // If the dial has gone high, increment the number we are trying to dial
        CurrentDialNumber++;
        DialTimeoutTime = millis();           // And reset the timeout
      }      
    }
    
    DialTime = millis();                      // Reset the timeout
  }

  if(millis() - CradleTime >= CradleSampleDelay){   // If we have exceeded the time between cradle samples
    if(digitalRead(CradlePin) != CradleState){      // And the cradle state has changed
      Serial.print("CradleEvent: ");                // Send a cradle event
      CradleState = digitalRead(CradlePin);         // Update the state of the cradle
      Serial.println(CradleState ? "Down" : "Up");  // Send the type of the cradle event
    }
    
    CradleTime = millis();                          // Reset the timeout
  }

  if(millis() - DialTimeoutTime >= DialTimeout){ // If we have exceeded the timeout on the dial                    
    if(CurrentDialNumber != 0){                  // And there has been more than 1 dial pulse
      Serial.print("DialEvent: ");               // Send a dial event
      Serial.println(CurrentDialNumber);         // And the number dialled
      CurrentDialNumber = 0;                     // And reset the number
    } 

    DialTimeoutTime = millis();                  // Reset the timeout
  }
}
