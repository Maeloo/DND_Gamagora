/*

• Sauter : button Jump
• Sprint : boutton Sprint 
• Glisser : boutton Sprint + flèche du bas 
• S’accrocher : Automatique quand saut sur un bord / un mur
• Saut de mur : boutton Jump quand accrocher
• Baisser : flèche du bas
*/

KeyCode SprintButton = KeyCode.RightArrow;
KeyCode JumpButton = KeyCode.UpArrow;
KeyCode SlowDownButton = KeyCode.LeftArrow;
KeyCode DuckButton= KeyCode.DownArrow;

void Start(){
	
}
void Update(){
			if (Input.GetKey(SlowDownButton))
            {
			SlowDown();
            }


            else if (Input.GetKey(SprintButton) && !Input.GetKey(DuckButton))
            {
             Sprint(); 
            }

            else if (Input.GetKey(JumpButton))
            {
			Jump();
		
            }

            else if (Input.GetKey(DuckButton) && !Input.GetKey(SprintButton))
            {
			Duck();
            }

            else if (Input.GetKey(DuckButton) && Input.GetKey(SprintButton))
            {
			Slide();
            }
			
}

void SlowDown(){
	
}

void Sprint(){
	
}

void Jump(){
	
}

void Duck(){
	
}

void Slide(){
	
}