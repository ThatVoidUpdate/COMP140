int DialSampleDelay = 2;    //How long between samples of the dial
unsigned long DialTime = 0; //How long since the last toggle fo the dial pin
bool DialState = false;     //The current state of the dial
int CurrentDialNumber = 0;  //The current number being dialled
int DialTimeout = 200;      //How long from the last dialevent until the number is sent over serial
unsigned long DialTimeoutTime = 0;

int CradleSampleDelay = 2;
unsigned long CradleTime = 0;
bool CradleState = false;




void setup() {
  // put your setup code here, to run once:
  pinMode(2, INPUT_PULLUP);
  pinMode(3, INPUT_PULLUP);
  Serial.begin(9600);
}

void loop() {
  if(millis() - DialTime >= DialSampleDelay){
    //Serial.println(digitalRead(2));
    if(digitalRead(2) != DialState){
      DialState = digitalRead(2);
      
      if(DialState){
        CurrentDialNumber++;
        DialTimeoutTime = millis();
      }      
    }
    
    DialTime = millis();
  }

  if(millis() - CradleTime >= CradleSampleDelay){
    //Serial.println(digitalRead(2));
    if(digitalRead(3) != CradleState){
      Serial.print("CradleEvent: ");
      CradleState = digitalRead(3);
      Serial.println(CradleState ? "Down" : "Up");     
    }
    
    CradleTime = millis();
  }

  if(millis() - DialTimeoutTime >= DialTimeout){
    DialTimeoutTime = millis();
    if(CurrentDialNumber != 0){
      Serial.print("DialEvent: ");
      Serial.println(CurrentDialNumber);
      CurrentDialNumber = 0;
    }    
  }
}
