const byte interruptPin = 2; // Pin used for external interrupt

volatile int ElapsedTime = 0; // Elapsed time in milliseconds
volatile int ClickCount = 0; // Clicks since last output

void setup()
{
  Serial.begin(9600); // Start up serial communication
  pinMode(interruptPin, INPUT_PULLUP); // Set the interrupt pin to an output, pulled high
  attachInterrupt(digitalPinToInterrupt(interruptPin), Interrupted, FALLING); // Call the function "Interrupted" whenever the interrupt pin goes low

  OCR0A = 0x00; // Set up the timer0 overflow
  TIMSK0 |= _BV(OCIE0A); // "
}

void loop(){
  //nothing
}

void Interrupted() 
{
  ElapsedTime = 0; // Set the time since last output to 0
  ClickCount++; // Increase the amount of clicks by 1
  Serial.println("Interrupted!"); // Debug output
}

ISR(TIMER0_COMPA_vect) 
{ // Called 1000 times per second
  ElapsedTime++; // Increase the elapsed time
  if(ElapsedTime == 1000)
  { // If exactly 1 second has passed...
    ElapsedTime = 0; // Reset the time
    Serial.println(ClickCount); // Print out the amount of clicks
    ClickCount = 0; // Reset the click count
  }  
}
