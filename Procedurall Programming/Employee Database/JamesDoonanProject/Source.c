#include<stdio.h>

void main()
{
	//James Doonan - G00310428
	
	//Declare Variables
	float numOfHours = 0;
	float hourlyRate = 0;
	float employeeNumber = 0;
	float totalSalary;
	float aboveHours = 0;
	float companySalary = 0;
	float largestHours = 0;
	float mostHoursEmployee = 0;
	

	//Step 1 Loop - Initialise Loop Control

	printf("Please enter your employee number or -1 to terminate\n");
	scanf("%f", &employeeNumber);

	
	while (employeeNumber != -1)
	{
		
		//Read inputs

		printf("Please enter number of hours worked\n");
		scanf("%f", &numOfHours);

		printf("Please enter your hourly rate\n");
		scanf("%f", &hourlyRate);

		//debug test
		printf("********************************************************************\n");
		printf("Debug %.2f %.2f %.2f\n", employeeNumber, numOfHours, hourlyRate);
		
		
	
			//if statement to work out calculations depending on hours entered.
			if (numOfHours > 0 && numOfHours < 168)

			{
				if (numOfHours >= 0 && numOfHours <= 39)
				{
					totalSalary = numOfHours * hourlyRate;
					//adding this if statement updates the employee number to coinside with the highest hours worked
					if (numOfHours >= largestHours)
					{
						mostHoursEmployee = employeeNumber;
					}
				}
				else if (numOfHours > 39 && numOfHours <= 50)
				{
					totalSalary = (39 * hourlyRate) + (((numOfHours - 39) * (1.5f * hourlyRate)));
					aboveHours += numOfHours - 39;
					//adding this if statement updates the employee number to coinside with the highest hours worked
					if (numOfHours >= largestHours)
					{
						  mostHoursEmployee = employeeNumber;
					}
				}
				else if (numOfHours > 50 && numOfHours <= 168)
				{
					totalSalary = (39 * hourlyRate) + (11 * 1.5f * hourlyRate) + ((numOfHours - 50) * (2 * hourlyRate));
					aboveHours += numOfHours - 39;
					//adding this if statement updates the employee number to coinside with the highest hours worked
					if (numOfHours >= largestHours)
					{
						mostHoursEmployee = employeeNumber;
					}
				}
				//prints off the statement each time
				printf("********************************************************************\n");
				printf("Employee number is:  %.f\n", employeeNumber);
				printf("total salary for the week is:  %.2f\n", totalSalary);
				printf("********************************************************************\n");

				//calculates the total wages for the week
				companySalary += totalSalary;

				//this gives the largest number of hours that week
				if (numOfHours >= largestHours)
				{ 
					largestHours = numOfHours;
				}
				
			}
			else
				//an employee cant work less that 0 hours and also more than 168 hours a week becuase thats not possible
			{
				printf("not a valid number, too few or many hours entered\n");
			}
	
			

			//resets the loop 
			printf("Please enter your employee number or -1 to terminate\n");
			scanf("%f", &employeeNumber);
			
			
	}

	//Display the results
	printf("********************************************************************\n");
	printf("The total company wages bill:  %.2f\n", companySalary);
	printf("Total number of hours worked above the standard 39 hours per week was: %.2f\n", aboveHours);
	printf("The largest amout of hours worked was: %.2f  and the employee with the most hours worked per week was: %.2f\n", largestHours,mostHoursEmployee);
	printf("********************************************************************\n");
	


	



}