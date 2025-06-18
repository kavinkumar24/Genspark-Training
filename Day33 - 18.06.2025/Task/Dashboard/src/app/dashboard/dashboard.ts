import { Component, OnInit } from '@angular/core';
import { AddUserService } from '../services/User.service';
import { UserModel } from '../models/usermodel';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Chart, PieController, ArcElement, BarController, BarElement, CategoryScale, LinearScale, Tooltip, Legend } from 'chart.js';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
  standalone: true,
  imports: [FormsModule, CommonModule]
})
export class Dashboard implements OnInit {
  users: UserModel[] = [];
  filteredUsers: UserModel[] = [];
  selectedGender: string = '';
  selectedRole: string = '';
  selectedState: string = '';
  today = new Date();
  isLoading: boolean = false;
  constructor(private userService: AddUserService) {}

  ngOnInit() {

  Chart.register(PieController, ArcElement,BarController, BarElement, CategoryScale, LinearScale, Tooltip, Legend)
  setInterval(() => this.today = new Date(), 1000); 
  this.isLoading = true;
  this.userService.getUsers().subscribe({
    next: res => {
      this.users = res.users;
      this.filteredUsers = this.users;
      this.isLoading = false;

      setTimeout(() => {
        this.createRoleBarChart();
        this.createGenderChart();
      }, 100);
    },
    error: err => {
      console.error('Error fetching users:', err);
      this.isLoading = false;
      alert('Failed to load users. Please try again later.');
    }
  });
}

  filterUsers() {
    this.filteredUsers = this.users.filter(user =>
      (this.selectedGender ? user.gender === this.selectedGender : true) &&
      (this.selectedRole ? user.role === this.selectedRole : true) &&
      (this.selectedState ? user.address.state === this.selectedState : true)
    );

    setTimeout(()=> {
      this.createRoleBarChart();
      this.createGenderChart();
    }, 100);
  }

  get genderCounts() {
  const counts: { [key: string]: number } = {};
  for (const user of this.filteredUsers) {
    counts[user.gender] = (counts[user.gender] || 0) + 1;
  }
    return counts;
  }

  get roleCounts(){
      const counts: { [key: string]: number } = {};
      for(const user of this.filteredUsers){
        counts[user.role] = (counts[user.role] || 0) +1;
      }
      return counts;

  }

  createRoleBarChart() {
   const roleLabels = Object.keys(this.roleCounts);
   const roleData = Object.values(this.roleCounts);

   const existingCanvas = Chart.getChart("roleBarChart");
   if (existingCanvas) {
     existingCanvas.destroy();
   }
   const ctx = document.getElementById('roleBarChart') as HTMLCanvasElement;
   new Chart(ctx, {
     type: 'bar',
     data: {
       labels: roleLabels,
       datasets: [{
         label: 'User Roles',
         data: roleData,
         backgroundColor: ['#FBCFE8', '#DDD6FE', '#BFDBFE'],
         borderRadius:10
       }]
     },
     options: {
       responsive: true,
       plugins: {
         legend: {
           display: false
         }
       },
       scales: {
         y: {
           beginAtZero: true
         }
       }
     }
   });
 }

 createGenderChart(){
  const genderlabels = Object.keys(this.genderCounts);
  const genderData = Object.values(this.genderCounts);

  const existingChart = Chart.getChart("genderPieChart");
  if(existingChart){
    existingChart.destroy();
  }

  const ctx = document.getElementById('genderPieChart') as HTMLCanvasElement;
   new Chart(ctx, {
     type: 'pie',
     data: {
       labels: genderlabels,
       datasets: [{
         label: 'Gender Distribution',
         data: genderData,
         backgroundColor: ['#FCA5A5', '#93C5FD', '#36a2eb'],
         borderWidth:1
       }]
     },
     options: {
       responsive: true,
       maintainAspectRatio:false,
       plugins: {
         legend: {
           position:'bottom',
           labels:{
            color:'#333',
            font:{
              size:14
            }
           }
         }
       },
       
     }
   });
 }

}