#if false
int S4FUNCTION d4append_blank( DATA4 *data )
{
   int rc ;

   rc = d4append_start( data, 0 ) ;  /* updates the record, returns -1 if code_base->error_code < 0 */
   if ( rc )
      return rc ;

   memset( data->record, ' ', data->record_width ) ;
   return d4append( data ) ;
}

int S4FUNCTION d4append_data( DATA4 *data )
{
   long count, pos ;
   int  rc ;

   count = d4reccount( data ) ;  /* returns -1 if code_base->error_code < 0 */
   if ( count < 0L ) 
      return -1 ;
   data->file_changed = 1 ;
   pos = d4record_position( data, count + 1L ) ;
   data->record[data->record_width] = 0x1A ;

   rc = file4write( &data->file, pos, data->record, ( data->record_width + 1 ) ) ;
   if ( rc >= 0 )
   {
      data->rec_num = count + 1L ;
      data->record_changed = 0 ;
//      #ifndef S4SINGLE
         if ( d4lock_test_append( data ) )
//      #endif  /* S4SINGLE */
      data->num_recs = count + 1L ;
   }

   data->record[data->record_width] = 0 ;

   return rc ;
}

int  S4FUNCTION d4append_start( DATA4 *data, int use_memo_entries )
{
   int rc, i ;
   char *save_ptr ;

   rc = d4update_record( data, 1 ) ;   /* returns -1 if code_base->error_code < 0 */
   if ( rc )
      return rc ;

   if ( data->rec_num <= 0 )
      use_memo_entries = 0 ;

   data->rec_num = 0 ;
   return 0 ;
}
#endif