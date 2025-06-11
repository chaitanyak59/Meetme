import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MembersDetailComponent } from './members/members-detail/members-detail.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { authGuardGuard } from './guards/auth-guard.guard';

export const routes: Routes = [
    { path: '', component: HomeComponent, title: 'Home' },
    {
        path: '',
        canActivate: [authGuardGuard],
        runGuardsAndResolvers: 'always',
        children: [
            { path: 'members', component: MembersListComponent, title: 'Members' },
            { path: 'members/:id', component: MembersDetailComponent },
            { path: 'lists', component: ListsComponent },
            { path: 'messages', component: MessagesComponent, title: 'Messages' },
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
