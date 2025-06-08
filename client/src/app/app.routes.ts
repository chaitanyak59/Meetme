import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MembersDetailComponent } from './members/members-detail/members-detail.component';
import { MessagesComponent } from './messages/messages.component';

export const routes: Routes = [
    {path: '', component: HomeComponent, title: 'Home'},
    {path: 'members', component: MembersListComponent, title: 'Members'},
    {path: 'members/:id', component: MembersDetailComponent },
    {path: 'messages', component: MessagesComponent, title: 'Messages'},
    {path: '**', redirectTo: '', pathMatch: 'full'} 
];
